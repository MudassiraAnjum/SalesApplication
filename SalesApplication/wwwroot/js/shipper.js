document.addEventListener("DOMContentLoaded", function () {
    // Function to get the auth token from localStorage
    function getAuthToken() {
        return localStorage.getItem("authToken");
    }

    // Handle Create Shipper
    document.getElementById("createShipperForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const shipperData = {
            companyName: document.getElementById("companyName").value,
            phone: document.getElementById("phone").value,
            email: document.getElementById("email").value,
            password: document.getElementById("password").value,
        };

        fetch(`/api/shipper`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}` // Include the token here
            },
            body: JSON.stringify(shipperData),
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(err => { throw new Error(err); });
                }
                return response.json();
            })
            .then(data => {
                alert(`Shipper created successfully: ${JSON.stringify(data)}`);
            })
            .catch(error => {
                console.error("Error creating shipper:", error);
                alert("Error creating shipper: " + error.message);
            });
    });

    // Handle Update Shipper
    document.getElementById("updateShipperForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const shipperId = document.getElementById("shipperId").value;

        let patchData;
        try {
            patchData = JSON.parse(document.getElementById("patchData").value);
        } catch (error) {
            alert("Invalid JSON format in Patch Data. Please correct it and try again.");
            console.error("Error parsing JSON patch data:", error);
            return;
        }

        fetch(`/api/shipper/edit/${shipperId}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json-patch+json',
                'Authorization': `Bearer ${getAuthToken()}` // Include the token here
            },
            body: JSON.stringify(patchData),
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(err => { throw new Error(err); });
                }
                return response.status === 204 ? null : response.json();
            })
            .then(data => {
                if (data) {
                    alert(`Shipper updated successfully: ${JSON.stringify(data)}`);
                } else {
                    alert("Shipper updated successfully!");
                }
            })
            .catch(error => {
                console.error("Error updating shipper:", error);
                alert("Error updating shipper: " + error.message);
            });
    });

    // Handle Get All Shippers
    document.getElementById("getAllShippers").addEventListener("click", function () {
        fetch(`/api/shipper`, {
            headers: {
                'Authorization': `Bearer ${getAuthToken()}` // Include the token here
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(err => { throw new Error(err); });
                }
                return response.json();
            })
            .then(data => {
                document.getElementById("allShippersResult").innerHTML = `
                    <h3>All Shippers</h3>
                    <pre>${JSON.stringify(data, null, 2)}</pre>
                `;
            })
            .catch(error => {
                console.error("Error fetching shippers:", error);
                alert("Error fetching shippers: " + error.message);
            });
    });

    // Handle Get Total Earnings
    document.getElementById("totalEarningsForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const date = document.getElementById("earningsDate").value;

        fetch(`/api/shipper/totalamountearnedbyshipper/${date}`, {
            headers: {
                'Authorization': `Bearer ${getAuthToken()}` // Include the token here
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(err => { throw new Error(err); });
                }
                return response.json();
            })
            .then(data => {
                document.getElementById("totalEarningsResult").innerHTML = `
                    <h3>Total Earnings on ${date}</h3>
                    <pre>${JSON.stringify(data, null, 2)}</pre>
                `;
            })
            .catch(error => {
                console.error("Error fetching earnings:", error);
                alert("Error fetching earnings: " + error.message);
            });
    });
});