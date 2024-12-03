function getOrdersByEmployee() {
    const firstName = document.getElementById("firstName").value;

    // Function to get the auth token from localStorage
    function getAuthToken() {
        return localStorage.getItem("authToken");
    }

    fetch(`https://localhost:7141/api/order/orderbyemployee/${firstName}`, {
        method: 'GET', // Specify the method (GET is the default for fetch)
        headers: {
            'Authorization': `Bearer ${getAuthToken()}` // Include the token in the headers
        }
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(err => { throw new Error(err); }); // Handle errors
            }
            // Check if response is empty
            if (response.status === 204) {
                throw new Error("No content returned for this request."); // Handle no content case
            }
            return response.json(); // Parse the response as JSON
        })
        .then(data => {
            if (data.length === 0) {
                document.getElementById("ordersResult").innerHTML = "No orders found for this employee.";
            } else {
                document.getElementById("ordersResult").innerHTML = `<pre>${JSON.stringify(data, null, 2)}</pre>`;
            }
        })
        .catch(error => {
            document.getElementById("ordersResult").innerHTML = "Error fetching data: " + error.message;
        });
}