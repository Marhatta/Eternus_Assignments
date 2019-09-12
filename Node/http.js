const http = require('http');

let courses = {
    "courses":[
        {
            "name":"ReactJs",
            "id":"1"
        },
        {
            "name":"NodeJS",
            "id":"2"
        }
    ]
}

const server = http.createServer((req,res) => {
    if(req.url === '/api/courses') {
            res.write(JSON.stringify(courses));
            res.end();
    }
})

server.listen(3002);
console.log('Listening at port 3002');