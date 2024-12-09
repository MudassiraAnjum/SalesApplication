const apiBaseUrl = "http://localhost:7141/api/shipper"; // Update with your API URL

// Get Shipper Details by Company Name
/*document.getElementById("getShipperBtn").addEventListener("click", async () => {
    const companyName = document.getElementById("companyName").value.trim();
    const errorMessage = document.getElementById("errorMessage");
    const shipperInfo = document.getElementById("shipperInfo");

    errorMessage.textContent = ""; // Clear previous errors
    shipperInfo.innerHTML = ""; // Clear previous results

    if (!companyName) {
        errorMessage.textContent = "Please enter a company name.";
        return;
    }

    try {
        //const response = await fetch(`${apiBaseUrl}/${encodeURIComponent(companyName)}`, { method: "GET" });
        //const response = await fetch('https://localhost:7141/api/Shipper/${companyName}');
        //console.log(response);
        const token = localStorage.getItem('authToken');;
        const url = `https://localhost:7141/api/Shipper/${companyName}`;

        fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log(data);
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });

        if (response.ok) {
            const data = await response.json();
            shipperInfo.innerHTML = `
                <p><strong>Company Name:</strong> ${data.companyName || "N/A"}</p>
                <p><strong>Phone:</strong> ${data.phone || "N/A"}</p>
                <p><strong>Email:</strong> ${data.email || "N/A"}</p>
            `;
        } else if (response.status === 404) {
            errorMessage.textContent = "Shipper not found.";
        } else {
            errorMessage.textContent = "Error fetching shipper details. Please try again.";
        }
    } catch (error) {
        console.error("Network error:", error);
        errorMessage.textContent = "Failed to fetch shipper details.";
    }
});
*/
document.getElementById("getShipperBtn").addEventListener("click", async () => {
    const companyName = document.getElementById("companyName").value.trim();
    const errorMessage = document.getElementById("errorMessage");
    const shipperInfo = document.getElementById("shipperInfo");

    errorMessage.textContent = ""; // Clear previous errors
    shipperInfo.innerHTML = ""; // Clear previous results

    if (!companyName) {
        errorMessage.textContent = "Please enter a company name.";
        return;
    }

    try {
        const token = localStorage.getItem('authToken');
        const url = `https://localhost:7141/api/Shipper/${companyName}`;

        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const data = await response.json();
            shipperInfo.innerHTML = `
                <p><strong>Company Name:</strong> ${data.companyName || "N/A"}</p>
                <p><strong>Phone:</strong> ${data.phone || "N/A"}</p>
                <p><strong>Email:</strong> ${data.email || "N/A"}</p>
            `;
        } else if (response.status === 404) {
            errorMessage.textContent = "Shipper not found.";
        } else {
            errorMessage.textContent = "Error fetching shipper details. Please try again.";
        }
    } catch (error) {
        console.error("Network error:", error);
        errorMessage.textContent = "Failed to fetch shipper details.";
    }
});

// Get Total Earnings by Year
//document.getElementById("getEarningsBtn").addEventListener("click", async () => {
//    const year = document.getElementById("year").value.trim();
//    const errorMessage = document.getElementById("errorMessage");
//    const earningsInfo = document.getElementById("earningsInfo");

//    errorMessage.textContent = ""; // Clear previous errors
//    earningsInfo.innerHTML = ""; // Clear previous results

//    if (!year || isNaN(year)) {
//        errorMessage.textContent = "Please enter a valid year.";
//        return;
//    }

//    try {
//        const token = localStorage.getItem('authToken');
//        const url = `https://localhost:7141/api/Shipper/${year}`;

//        const response = await fetch(url, {
//            method: 'GET',
//            headers: {
//                'Authorization': `Bearer ${token}`
//            }
//        });
//        if (response.ok) {
//            const data = await response.json();
//            if (data.length > 0) {
//                earningsInfo.innerHTML = data.map((shipper) => `
//                    <p><strong>Company Name:</strong> ${shipper.companyName || "N/A"}</p>
//                    <p><strong>Total Amount Earned:</strong> $${shipper.totalAmount.toFixed(2)}</p>
//                    <hr>
//                `).join("");
//            //} else if{
//            //    earningsInfo.innerHTML = "No earnings found for the given year.";
//            }
//        } else {
//            errorMessage.textContent = "Error fetching earnings. Please try again.";
//        }
//    } catch (error) {
//        console.error("Network error:", error);
//        errorMessage.textContent = "Failed to fetch earnings.";
//    }
//});


//document.getElementById("getEarningsBtn").addEventListener("click", async () => {
//    const year = document.getElementById("year").value.trim();
//    const errorMessage = document.getElementById("errorMessage");
//    const earningsInfo = document.getElementById("earningsInfo");

//    errorMessage.textContent = ""; // Clear previous errors
//    earningsInfo.innerHTML = ""; // Clear previous results

//    if (!year) {
//        errorMessage.textContent = "Please enter a valid year.";
//        return;
//    }

//    try {
//        const token = localStorage.getItem('authToken');
//        const url = `http://localhost:7141/api/Shipper/totalamountearnedbyshipper/${year}`;


//        const response = await fetch(url, {
//            method: 'GET',
//            headers: {
//                'Authorization': `Bearer ${token}`
//            }
//        });

//        console.log('Response status:', response.status);
//        console.log('Response headers:', response.headers);

//        if (response.ok) {
//            const data = await response.json();
//            console.log('Response data:', data);
//            if (data.length > 0) {
//                earningsInfo.innerHTML = data.map((shipper) => `
//                    <p><strong>Company Name:</strong> ${shipper.companyName || "N/A"}</p>
//                    <p><strong>Total Amount Earned:</strong> $${shipper.totalAmount.toFixed(2)}</p>
//                    <hr>
//                `).join("");
//            } else {
//                earningsInfo.innerHTML = "Earnings not found for the given year.";
//            }
//        } else if (response.status === 404) {
//            errorMessage.textContent = "Earnings not found for the given year.";
//        } else {
//            const errorText = await response.text();
//            console.error('Error response text:', errorText);
//            errorMessage.textContent = "Error fetching earnings details. Please try again.";
//        }
//    } catch (error) {
//        console.error("Network error:", error);
//        errorMessage.textContent = "Failed to fetch earnings details.";
//    }
//});


document.getElementById("getEarningsBtn").addEventListener("click", async () => {
    const year = document.getElementById("year").value.trim();
    const errorMessage = document.getElementById("errorMessage");
    const earningsInfo = document.getElementById("earningsInfo");

    errorMessage.textContent = ""; // Clear previous errors
    earningsInfo.innerHTML = ""; // Clear previous results

    if (!year || isNaN(year)) {
        errorMessage.textContent = "Please enter a valid year.";
        return;
    }

    try {
        const token = localStorage.getItem('authToken');
        console.log('Token:', token); // Log the token to verify it
        const url = `https://localhost:7141/api/Shipper/${year}`;
        console.log('URL:', url); // Log the URL to verify it

        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        console.log('Response status:', response.status);
        console.log('Response headers:', response.headers);

        if (response.ok) {
            const data = await response.json();
            console.log('Response data:', data);
            if (data.length > 0) {
                earningsInfo.innerHTML = data.map((shipper) => `
                    <p><strong>Company Name:</strong> ${shipper.companyName || "N/A"}</p>
                    <p><strong>Total Amount Earned:</strong> $${shipper.totalAmount.toFixed(2)}</p>
                    <hr>
                `).join("");
            } else {
                earningsInfo.innerHTML = "Earnings not found for the given year.";
            }
        } else if (response.status === 404) {
            errorMessage.textContent = "Earnings not found for the given year.";
        } else {
            const errorText = await response.text();
            console.error('Error response text:', errorText);
            errorMessage.textContent = "Error fetching earnings details. Please try again.";
        }
    } catch (error) {
        console.error("Network error:", error);
        errorMessage.textContent = "Failed to fetch earnings details.";
    }
});

