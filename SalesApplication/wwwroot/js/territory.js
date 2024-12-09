function getTerritories() {
    // Function to get the auth token from localStorage
    function getAuthToken() {
        return localStorage.getItem("authToken");
    }

    fetch('https://localhost:7141/api/territory', {
        method: 'GET', // Specify the method (GET is the default for fetch)
        headers: {
            'Authorization': `Bearer ${getAuthToken()}` // Include the token in the headers
        }
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(err => { throw new Error(err); }); // Handle errors
            }
            return response.json(); // Parse the response as JSON
        })
        .then(data => {
            // Generate table
            let output = `
                <h2>Territories</h2>
                <table border="1" style="border-collapse: collapse; width: 100%;">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Description</th>
                        </tr>
                    </thead>
                    <tbody>
            `;

            data.forEach(territory => {
                output += `
                    <tr>
                        <td>${territory.territoryId}</td>
                        <td>${territory.territoryDescription}</td>
                    </tr>
                `;
            });

            output += `
                    </tbody>
                </table>
            `;

            document.getElementById("territoriesResult").innerHTML = output;
        })
        .catch(error => {
            document.getElementById("territoriesResult").innerHTML = "Error fetching data: " + error.message;
        });
}
