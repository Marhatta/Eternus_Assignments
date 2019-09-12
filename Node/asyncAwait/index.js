
notifyCustomer();

async function notifyCustomer(){
    const customer = await getCustomer(1);
    console.log('Customer: ',customer);
    if(customer.isGold){
        const movies =  await getTopMovies();
        console.log(' Top Movies: ',movies);
        await sendEmail(customer.email,movies);
        console.log('Email Sent successfully....');
    }
}


function getCustomer(id) {
    return new Promise((resolve,reject) => {[
        setTimeout(()=>{
            resolve({
                id,
                name:'vishal',
                isGold:true,
                email:'email'
            });
        },3000)
    ]});
}

function getTopMovies(){
    return new Promise((resolve,reject) => {
        setTimeout(() => {
            resolve(['movie1,movie2'])
        },3000);
    });
}

function sendEmail(email,movies){
    return new Promise((resolve,reject) => {
        setTimeout(() => {
            resolve();
        },3000)
    });
}