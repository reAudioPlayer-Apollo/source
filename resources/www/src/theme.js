if (window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches) {
    document.documentElement.setAttribute("data-theme", "dark");
}
window.mode = {}

window.mode.dark = function () {
    document.documentElement.setAttribute("data-theme", "dark");
}
window.mode.light = function () {
    document.documentElement.setAttribute("data-theme", "light");
}