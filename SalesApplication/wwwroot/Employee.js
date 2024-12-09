
const apiUrl = "https://localhost:7141/api/Employee";
const token = localStorage.getItem("authToken");

// Ensure the token exists
if (!token) {
    alert("Authentication token missing. Please log in again.");
    window.location.href = "/Auth.html";
}

// Set headers for API requests
const headers = {
    "Authorization": `Bearer ${token}`,
    "Content-Type": "application/json",
};

// Function to toggle visibility of sections
function showSection(sectionId) {
    document.querySelectorAll("[id$='-section']").forEach((section) => {
        section.style.display = section.id === sectionId ? "block" : "none";
    });
}

// Event listeners for navigation buttons
document.getElementById("get-employee-title").addEventListener("click", () => showSection("employee-title-section"));
document.getElementById("get-highest-sales-date").addEventListener("click", () => showSection("highest-sales-date-section"));
document.getElementById("get-highest-sales-year").addEventListener("click", () => showSection("highest-sales-year-section"));
document.getElementById("get-sales-by-employee").addEventListener("click", () => showSection("sales-by-employee-section"));

// Utility function to create a table
function createTable(data, headers) {
    const table = document.createElement("table");
    table.border = "1";
    table.style.width = "100%";
    table.style.borderCollapse = "collapse";

    // Create table header
    const thead = table.createTHead();
    const headerRow = thead.insertRow();
    headers.forEach((header) => {
        const th = document.createElement("th");
        th.textContent = header;
        headerRow.appendChild(th);
    });

    // Create table body
    const tbody = table.createTBody();
    data.forEach((row) => {
        const tr = tbody.insertRow();
        headers.forEach((header) => {
            const td = tr.insertCell();
            td.textContent = row[header.toLowerCase()] || "";
        });
    });

    return table;
}

// Fetch Employees by Title
document.getElementById("search-title-btn").addEventListener("click", async () => {
    const title = document.getElementById("employee-title-input").value.trim();
    if (!title) {
        alert("Please enter a title.");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/title/${encodeURIComponent(title)}`, { method: "GET", headers });
        if (response.ok) {
            const employees = await response.json();
            const listElement = document.getElementById("employee-title-list");
            listElement.innerHTML = "";

            if (employees.length > 0) {
                const headers = ["FirstName", "LastName", "Title", "BirthDate", "HireDate", "Address", "City"];
                const table = createTable(
                    employees.map((emp) => ({
                        firstname: emp.firstName || "",
                        lastname: emp.lastName || "",
                        title: emp.title || "",
                        birthdate: emp.birthDate ? new Date(emp.birthDate).toLocaleDateString() : "",
                        hiredate: emp.hireDate ? new Date(emp.hireDate).toLocaleDateString() : "",
                        address: emp.address || "",
                        city: emp.city || "",
                    })),
                    headers
                );
                listElement.appendChild(table);
            } else {
                listElement.innerHTML = "No employees found.";
            }
        } else {
            alert("Error fetching employees by title. Please try again.");
        }
    } catch (error) {
        console.error("Network error:", error);
        alert("Failed to fetch employees.");
    }
});


// Event listener for "Get Highest Sales by Date"
document.getElementById("get-sales-by-date-btn").addEventListener("click", async () => {
    const year = document.getElementById("sales-date-input").value; // Get the year from the input field

    if (!year) {
        alert("Please select a year.");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/highestsalebyemployee/${year}`, { method: "GET", headers });

        if (response.ok) {
            const employeeData = await response.json();
            console.log("Response Data:", employeeData); // Log response data for debugging

            const listElement = document.getElementById("highest-sales-date-list");
            listElement.innerHTML = ""; // Clear previous results

            if (employeeData && employeeData.length > 0) {
                // Create and append table here
                const headers = ["EmployeeId", "FirstName", "LastName", "Title", "BirthDate", "HireDate", "Address", "City", "Phone"];
                const table = createTable(
                    employeeData.map((emp) => ({
                        employeeid: emp.employeeId || "",
                        firstname: emp.firstName || "",
                        lastname: emp.lastName || "",
                        title: emp.title || "",
                        birthdate: emp.birthDate ? new Date(emp.birthDate).toLocaleDateString() : "",
                        hiredate: emp.hireDate ? new Date(emp.hireDate).toLocaleDateString() : "",
                        address: emp.address || "",
                        city: emp.city || "",
                        phone: emp.homePhone || "",
                    })),
                    headers
                );
                listElement.appendChild(table);
            } else {
                listElement.innerHTML = "No sales data found for the selected year.";
            }
        } else {
            alert(`Error fetching sales data: ${response.statusText}`);
        }
    } catch (error) {
        console.error("Error during fetch:", error);
        alert("Failed to fetch sales data. Please try again.");
    }
});


// JavaScript code to fetch highest sales by year and display the details in a list

document.getElementById("get-sales-by-year-btn").addEventListener("click", async () => {
    const year = document.getElementById("sales-year-input").value;

    if (!year) {
        alert("Please enter a year.");
        return;
    }

    try {

        // Send GET request to fetch the highest sales data by year
        const response = await fetch(`${apiUrl}/highestsalebyemployee/${year}`, { method: "GET", headers });

        if (response.ok) {
            const employeeData = await response.json();
            console.log("Employee Data:", employeeData);

            const listElement = document.getElementById("highest-sales-year-list");
            listElement.innerHTML = ""; // Clear previous results

            if (employeeData.length > 0) {
                // Iterate over the employee data and create a list for each employee
                employeeData.forEach(employee => {
                    const li = document.createElement("li");
                    li.innerHTML = `
                        <strong>Employee ID:</strong> ${employee.employeeId || "N/A"} <br>
                        <strong>Name:</strong> ${employee.firstName || ""} ${employee.lastName || ""} <br>
                        <strong>Title:</strong> ${employee.title || "N/A"} <br>
                        <strong>Title of Courtesy:</strong> ${employee.titleOfCourtesy || "N/A"} <br>
                        <strong>Birth Date:</strong> ${employee.birthDate ? new Date(employee.birthDate).toLocaleDateString() : "N/A"} <br>
                        <strong>Hire Date:</strong> ${employee.hireDate ? new Date(employee.hireDate).toLocaleDateString() : "N/A"} <br>
                        <strong>Address:</strong> ${employee.address || "N/A"} <br>
                        <strong>City:</strong> ${employee.city || "N/A"} <br>
                        <strong>Region:</strong> ${employee.region || "N/A"} <br>
                        <strong>Postal Code:</strong> ${employee.postalCode || "N/A"} <br>
                        <strong>Country:</strong> ${employee.country || "N/A"} <br>
                        <strong>Home Phone:</strong> ${employee.homePhone || "N/A"} <br>
                        <strong>Extension:</strong> ${employee.extension || "N/A"} <br><br>
                    `;
                    listElement.appendChild(li);
                });
            } else {
                listElement.innerHTML = "No sales data found for the selected year.";
            }
        } else {
            alert(`Error fetching sales data: ${response.statusText}`);
        }
    } catch (error) {
        console.error("Error during fetch:", error);
        alert("Failed to fetch sales data. Please try again.");
    }
});


document.getElementById("get-employee-sales-btn").addEventListener("click", async () => {
    const employeeId = document.getElementById("employee-id-input").value;
    const salesDate = document.getElementById("employee-sales-date-input").value;

    // Check if both Employee ID and Sales Date are provided
    if (!employeeId || !salesDate) {
        alert("Please enter both Employee ID and Date.");
        return;
    }


    try {
        const apiUrl = "https://localhost:7141/api/Employee"; // Replace with your actual API URL
        const headers = {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json",
        };

        // Send GET request to fetch sales data by employee ID and date
        const response = await fetch(`${apiUrl}/salesbyemployee/${employeeId}/${salesDate}`, { method: "GET", headers });

        if (response.ok) {
            const salesData = await response.json();
            console.log("Sales Data:", salesData);

            const listElement = document.getElementById("employee-sales-list");
            listElement.innerHTML = ""; // Clear previous results

            if (salesData && salesData.length > 0) {
                // Iterate over the sales data and create a list item for each order
                salesData.forEach(sale => {
                    const li = document.createElement("li");
                    li.innerHTML = `
                        <strong>Order ID:</strong> ${sale.orderId || "N/A"} <br>
                        <strong>Company Name:</strong> ${sale.companyName || "N/A"} <br>
                        <strong>Sales Date:</strong> ${salesDate || "N/A"} <br>
                        <strong>Employee ID:</strong> ${employeeId || "N/A"} <br><br>
                    `;
                    listElement.appendChild(li);
                });
            } else {
                listElement.innerHTML = "No sales data found for the selected employee and date.";
            }
        } else {
            alert(`Error fetching sales data: ${response.statusText}`);
        }
    } catch (error) {
        console.error("Error during fetch:", error);
        alert("Failed to fetch sales data. Please try again.");
    }
});
