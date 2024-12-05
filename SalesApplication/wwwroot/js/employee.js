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
                <pre>${JSON.stringify(data, null, 2)}</pre>
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
                <pre>${JSON.stringify(data, null, 2)}</pre>
            `;
            })
            .catch(error => {
                console.error("Error fetching sales data:", error);
                alert("Error fetching sales data: " + error.message);
            });
    });

});