function toggleMenu() {
    var x = document.getElementById("navBar");
    if (x.className === "nav") {
        x.className += " responsive";
    } else {
        x.className = "nav";
    }
}