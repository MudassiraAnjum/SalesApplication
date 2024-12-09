// API Base URL and token
const apiUrl = "https://localhost:7141/api/Territory/update";
const token = localStorage.getItem("authToken");

// Ensure the token exists
if (!token) {
    alert("Authentication token missing. Please log in again.");
    window.location.href = "/Auth.html"; // Redirect to login if token is missing
}

// Set headers for API requests
const headers = {
    "Authorization": `Bearer ${token}`,
    "Content-Type": "application/json",
};

// Function to handle the form submission for updating the territory
document.getElementById("update-territory-form").addEventListener("submit", async (event) => {
    event.preventDefault();

    // Get input values
    const territoryId = document.getElementById("territory-id").value.trim();
    const regionId = document.getElementById("region-id").value.trim();
    const territoryDescription = document.getElementById("territory-description").value.trim();

    // Validate inputs
    if (!territoryId || !regionId || !territoryDescription) {
        alert("Please fill in all fields.");
        return;
    }

    // Prepare the payload
    const payload = {
        RegionId: parseInt(regionId), // Ensure region ID is a number
        TerritoryDescription: territoryDescription,
    };

    console.log("Payload being sent:", payload); // Log the payload
    console.log("API URL:", `${apiUrl}/${encodeURIComponent(territoryId)}`); // Log the endpoint URL

    try {
        // Make the PUT request to update the territory
        const response = await fetch(`${apiUrl}/${encodeURIComponent(territoryId)}`, {
            method: "PUT",
            headers,
            body: JSON.stringify(payload),
        });

        const responseDiv = document.getElementById("territory-response");
        responseDiv.innerHTML = ""; // Clear previous responses

        if (response.ok) {
            const result = await response.json();
            console.log("Update successful:", result);
            responseDiv.innerHTML = `<p>Territory updated successfully: ${JSON.stringify(result)}</p>`;
        } else {
            const errorMessage = await response.text();
            console.error("Error response from server:", errorMessage);
            responseDiv.innerHTML = `<p>Error updating territory: ${errorMessage}</p>`;
        }
    } catch (error) {
        console.error("Network or unexpected error:", error);
        alert("An error occurred while updating the territory. Please try again.");
    }
});
