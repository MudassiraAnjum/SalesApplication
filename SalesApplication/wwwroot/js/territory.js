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
            let output = "<h2>Territories:</h2><ul>";
            data.forEach(territory => {
                output += `<li><strong>ID:</strong> ${territory.territoryId}<br><strong>Description:</strong> ${territory.territoryDescription}</li>`;
            });
            output += "</ul>";
            document.getElementById("territoriesResult").innerHTML = output;
        })
        .catch(error => {
            document.getElementById("territoriesResult").innerHTML = "Error fetching data: " + error.message;
        });
}