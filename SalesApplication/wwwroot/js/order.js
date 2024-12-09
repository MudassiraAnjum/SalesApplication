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
                // Generate table
                let table = `<table border="1" style="width: 100%; border-collapse: collapse;">
                                <thead>
                                    <tr>`;

                // Add headers dynamically based on the keys of the first object
                Object.keys(data[0]).forEach(key => {
                    table += `<th style="padding: 8px; text-align: left; background-color: #f2f2f2;">${key}</th>`;
                });

                table += `</tr>
                                </thead>
                                <tbody>`;

                // Add rows for each object
                data.forEach(order => {
                    table += `<tr>`;
                    Object.values(order).forEach(value => {
                        table += `<td style="padding: 8px; text-align: left;">${value}</td>`;
                    });
                    table += `</tr>`;
                });

                table += `</tbody></table>`;

                // Insert table into the existing ordersResult div
                document.getElementById("ordersResult").innerHTML = table;
            }
        })
        .catch(error => {
            document.getElementById("ordersResult").innerHTML = "Error fetching data: " + error.message;
        });
}
