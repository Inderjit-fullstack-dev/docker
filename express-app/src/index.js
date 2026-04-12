const express = require('express'); 

const port = 3000;
const app = express();
 
app.get('/users', (request, response) => {
    const users = [
        {
            id: 1,
            name: 'John Doe',
            email: 'john.doe@example.com',
            phone: '+1-555-0101',
            role: 'Admin'
        },
        {
            id: 2,
            name: 'Jane Smith',
            email: 'jane.smith@example.com',
            phone: '+1-555-0102',
            role: 'User'
        },
        {
            id: 3,
            name: 'Bob Johnson',
            email: 'bob.johnson@example.com',
            phone: '+1-555-0103',
            role: 'User'
        },
        {
            id: 4,
            name: 'Alice Brown',
            email: 'alice.brown@example.com',
            phone: '+1-555-0104',
            role: 'Editor'
        },
        {
            id: 5,
            name: 'Charlie Wilson',
            email: 'charlie.wilson@example.com',
            phone: '+1-555-0105',
            role: 'Viewer'
        }
    ];
    
    response.json(users);
});

app.listen(port, () => {
    console.log(`server running on port ${port}`);
});