document.addEventListener("DOMContentLoaded", function () {

    // Function to get the auth token from localStorage
    function getAuthToken() {
        return localStorage.getItem("authToken");
    }

    // Handle Delete Employee Form
    document.getElementById("deleteEmployeeForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const employeeId = document.getElementById("employeeId").value;

        fetch(`/api/employee?id=${employeeId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${getAuthToken()}` // Include the token in the headers
            },
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(err => { throw new Error(err.message); });
                }
                return response.json();
            })
            .then(data => {
                alert(`Employee deleted successfully: ${JSON.stringify(data)}`);
            })
            .catch(error => {
                console.error("Error deleting employee:", error);
                alert("Error deleting employee: " + error.message);
            });
    });

    // Handle Get Lowest Sale by Employee in Year
    document.getElementById("lowestSaleForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const year = document.getElementById("year").value;

        fetch(`/api/employee/lowestsalebyemployee/${year}`, {
            headers: {
                'Authorization': `Bearer ${getAuthToken()}` // Include the token in the headers
            },
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(err => { throw new Error(err.message); });
                }
                return response.json();
            })
            .then(data => {
                document.getElementById("lowestSaleResult").innerHTML = `
                <h3>Lowest Sale for Employee in ${year}</h3>
                ${generateTableHTML(data)}
            `;
            })
            .catch(error => {
                console.error("Error fetching lowest sale:", error);
                alert("Error fetching lowest sale: " + error.message);
            });
    });

    // Handle Get Sales Made by Employee Between Dates
    document.getElementById("salesBetweenDatesForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const employeeId = document.getElementById("employeeIdForSales").value;
        const fromDate = document.getElementById("fromDate").value;
        const toDate = document.getElementById("toDate").value;

        fetch(`/api/employee/Salemadebyanemployeebetweendates/${employeeId}/${fromDate}/${toDate}`, {
            headers: {
                'Authorization': `Bearer ${getAuthToken()}` // Include the token in the headers
            },
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(err => { throw new Error(err.message); });
                }
                return response.json();
            })
            .then(data => {
                document.getElementById("salesBetweenDatesResult").innerHTML = `
                <h3>Sales Made by Employee (ID: ${employeeId})</h3>
                ${generateTableHTML(data)}
            `;
            })
            .catch(error => {
                console.error("Error fetching sales data:", error);
                alert("Error fetching sales data: " + error.message);
            });
    });

    // Utility function to generate table HTML from JSON data
    function generateTableHTML(data) {
        if (!data || (Array.isArray(data) && data.length === 0)) {
            return "<p>No data available</p>";
        }

        let table = `<table border="1" style="width:100%; text-align:left;">`;

        // If the data is an array of objects
        if (Array.isArray(data)) {
            // Generate table headers
            const headers = Object.keys(data[0]);
            table += "<tr>";
            headers.forEach(header => {
                table += `<th>${header}</th>`;
            });
            table += "</tr>";

            // Generate table rows
            data.forEach(row => {
                table += "<tr>";
                Object.values(row).forEach(value => {
                    table += `<td>${value}</td>`;
                });
                table += "</tr>";
            });
        } else {
            // If the data is a single object
            const headers = Object.keys(data);
            table += "<tr>";
            headers.forEach(header => {
                table += `<th>${header}</th>`;
            });
            table += "</tr>";

            table += "<tr>";
            Object.values(data).forEach(value => {
                table += `<td>${value}</td>`;
            });
            table += "</tr>";
        }

        table += "</table>";
        return table;
    }
});
