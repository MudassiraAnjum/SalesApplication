const apiUrl = "https://localhost:7141/api/Auth"; // Your API base URL

// Show the login form and hide the register form and auth buttons
function showLoginForm() {
    document.getElementById("login-section").style.display = "block";
    document.getElementById("register-section").style.display = "none";
    document.getElementById("auth-buttons").style.display = "none"; // Hide the auth buttons when the form is shown
}

// Show the register form and hide the login form and auth buttons
function showRegisterForm() {
    document.getElementById("register-section").style.display = "block";
    document.getElementById("login-section").style.display = "none";
    document.getElementById("auth-buttons").style.display = "none"; // Hide the auth buttons when the form is shown
}

// Show the buttons and hide the logout button after logout
function showAuthButtons() {
    document.getElementById("auth-buttons").style.display = "block"; // Show login and register buttons
    document.getElementById("logout-btn").style.display = "none"; // Hide logout button
}

// Register AJAX call
document.getElementById("register-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const companyName = document.getElementById("register-company").value;
    const email = document.getElementById("register-email").value;
    const password = document.getElementById("register-password").value;

    try {
        const response = await fetch(`${apiUrl}/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ companyName, email, password })
        });

        if (response.ok) {
            document.getElementById("register-response").textContent = "Registration successful. Redirecting to login...";
            setTimeout(() => {
                showLoginForm(); // Show the login form after successful registration
            }, 2000);
        } else {
            const error = await response.text();
            document.getElementById("register-response").textContent = `Error: ${error}`;
        }
    } catch (error) {
        document.getElementById("register-response").textContent = `Error: ${error.message}`;
    }
});

// Login AJAX call with role-based redirection
document.getElementById("login-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const username = document.getElementById("login-username").value;
    const password = document.getElementById("login-password").value;

    try {
        const response = await fetch(`${apiUrl}/login?username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {
            const { token, role } = await response.json();
            localStorage.setItem("authToken", token); // Store the token in localStorage
            localStorage.setItem("userRole", role);    // Store the role

            // Redirect based on role
            if (role === "Admin") {
                window.location.href = "/index.html"; // Redirect to admin page (index.html)
            } else if (role === "Employee") {
                window.location.href = "/index2.html"; // Redirect to employee page
            } else if (role === "Shipper") {
                window.location.href = "/index3.html"; // Redirect to shipper page
            } else {
                alert("Invalid role.");
            }
        } else if (response.status === 401) {
            document.getElementById("login-response").textContent = "Invalid username or password.";
        } else {
            const error = await response.text();
            document.getElementById("login-response").textContent = `Error: ${error}`;
        }
    } catch (error) {
        document.getElementById("login-response").textContent = `Error: ${error.message}`;
    }
});

// Logout function
function logout() {
    // Remove token and role from localStorage
    localStorage.removeItem("authToken");
    localStorage.removeItem("userRole");

    // Redirect to login page after logout
    window.location.href = "/html/auth.html"; // Change to your desired landing page
}

// Run the authorization check and adjust visibility on page load
window.addEventListener("load", () => {
    checkAuthorization();

    const logoutBtn = document.getElementById("logout-btn");
    const token = localStorage.getItem("authToken");

    // If token exists, show the logout button and hide the auth buttons
    if (token) {
        logoutBtn.style.display = "inline-block"; // Show logout button
        document.getElementById("auth-buttons").style.display = "none"; // Hide login/register buttons
    } else {
        showAuthButtons(); // Show auth buttons if not logged in
        logoutBtn.style.display = "none"; // Hide logout button
    }

    // Add event listeners for the buttons
    const loginBtn = document.getElementById("login-btn");
    const newShipperBtn = document.getElementById("new-shipper-btn");

    loginBtn.addEventListener("click", () => {
        showLoginForm();  // Show login form when clicking on login button
    });

    newShipperBtn.addEventListener("click", () => {
        showRegisterForm();  // Show register form when clicking on new shipper button
    });
});

// Check if the user is authorized to access the current page
function checkAuthorization() {
    const token = localStorage.getItem("authToken");
    const currentPath = window.location.pathname;

    // Redirect to the landing/auth page if accessing a secured page without a token
    if (!token && (currentPath.includes("shipper.html") || currentPath.includes("employee.html") || currentPath.includes("index.html"))) {
        alert("Unauthorized access. Redirecting to login.");
        window.location.href = "Auth.html"; // Redirect to the landing/auth page
    }
}