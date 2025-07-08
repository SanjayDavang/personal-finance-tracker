document.getElementById("categoryInput").addEventListener("input", async function () {
    let query = this.value.trim().toLowerCase();
    let suggestionsBox = document.getElementById("suggestions");

    suggestionsBox.innerHTML = "";

    if (query.length < 1) {
        suggestionsBox.innerHTML = "";
        return;
    }

    try {
        const response = await fetch(`/Category/SearchCategories?query=${query}`);
        const data = await response.json();

        if (data.length > 0) {
            suggestionsBox.innerHTML = data.map(c => {
                let highlightedName = highlightText(c.name, query);
                return `<li class="list-group-item list-group-item-action" onclick="selectCategory('${c.name}')">${highlightedName}</li>`;
            }).join('');
        } 
    } catch (error) {
        console.error("Error fetching categories:", error);
    }
});

function selectCategory(categoryName) {
    document.getElementById("categoryInput").value = categoryName; // Set input value
    document.getElementById("suggestions").innerHTML = ""; // Clear suggestions
}

function highlightText(text, query) {
    const regex = new RegExp(`(${query})`, "gi");
    return text.replace(regex, "<strong>$1</strong>");
}
