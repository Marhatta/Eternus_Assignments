const sql = require('mssql/msnodesqlv8');
const config = require('./config');
const bodyParser = require('body-parser');
const Joi = require('@hapi/joi');
const express = require('express');
const app = express();


app.use(bodyParser.json());


//Define Joi schema for validation
const schema = {
    name:Joi.string().min(3).required(),
    price:Joi.number().min(1).required(),
    url:Joi.string().regex(/^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/).required()
}

//close the connection
const close = () => sql.close();


//query Execution
const executeQuery = (query,res) => {
    close();
    //connect to your database
    sql.connect(config, function (err) {
      if (err) {
          console.log(err);
          close();
      }
     // create Request object
    const request = new sql.Request();
           
    // query to the database 
    request.query(query,(err, record) => {
            
            if (err) {
                res.status(503);//service unavailable
                console.log(err);
                close();
            }
            res.send(record);
            close(); 
         });
   });
}

//get all products
app.get('/api/products', function (req, res) {
    const query = 'Select * from product';
    executeQuery(query,res);
});

//get a product with specific id
app.get('/api/products/:id',(req,res) => {
    const query = `select * from product where id = ${req.params.id}`;
    executeQuery(query,res);
});


//post a product
app.post('/api/products',(req,res) => {

    const product = {
       name:req.body.name,
       price:req.body.price,
       url:req.body.url
    }

    Joi.validate(product,schema,(err,value) => {
        if(err === null){
            const query = `insert into product values
                            ('${product.name}',${product.price},'${product.url}')`;
            executeQuery(query,res);
        }
        else{
            res.status(400).send(err.details[0].message);
        }
    });
})



//Delete a product
app.delete('/api/products/:id',(req,res) => {
    const query = `delete from product where id=${req.params.id}`;
    executeQuery(query,res);
})

//update a product
app.put('/api/products/:id',(req,res) => {

     const product = {
         name:req.body.name,
        price:req.body.price,
        url:req.body.url
    }
    
    Joi.validate(product,schema,(err,value) => {
        if(err === null){
            const query = `update product set name='${product.name}' , 
                            price=${product.price} , url='${product.url}'
                            where id=${req.params.id}`;

            executeQuery(query,res);
        }
        else{
            res.status(400).send(err.details[0].message);
        }
    })
    
})

app.listen(5000, function () {
    console.log('Server is running at port 5000..');
});