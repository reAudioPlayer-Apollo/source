function getImageFromSrc(src) {
    return img = "url('" + src + "')"
}

window.getImage = function getImage(url) {
    var pic = getSrc(url)
    return img = getImageFromSrc(pic)
}

window.getSrc = function getSrc(data) {
    return "data:image/gif;base64," + data
}

function setCover(evt) {
    var pic = window.getSrc(evt.data.data)
    var img = getImageFromSrc(pic)
    document.getElementById("back").style.backgroundImage = img
    document.getElementById("ccover").src = pic
}

document.addEventListener("ws.connect", () => {
    ws.data.cover();
    ws.data.displayname();
    ws.data.volume();
});

document.addEventListener("ws.data.cover", setCover);
document.addEventListener("ws.data.displayname", (evt) => document.title = evt.data.data);
document.addEventListener("ws.data.volume", (evt) => document.getElementById("volumeBar").value = evt.data.data);

document.getElementById("accent").style.backgroundColor = hexToRGB("#000000", .3)

function hexToRGB(hex, alpha) {
    var r = parseInt(hex.slice(1, 3), 16),
        g = parseInt(hex.slice(3, 5), 16),
        b = parseInt(hex.slice(5, 7), 16);

    if (alpha) {
        return "rgba(" + r + ", " + g + ", " + b + ", " + alpha + ")";
    } else {
        return "rgb(" + r + ", " + g + ", " + b + ")";
    }
}

base = window.location.protocol + "//" + window.location.hostname + ":" + window.location.port + "/"
console.log(base)

document.getElementById("cover").addEventListener('animationiteration', animEnd)
document.getElementById("cover").addEventListener('webkitAnimationEnd', animEnd)
document.getElementById("ccover").addEventListener('click', playPause)

document.addEventListener("ws.control.playPause", (evt) => {
    document.getElementById("playPause").src = window.getSrc(evt.data.data);
    animPlayPause();
})

function animEnd(e) {
    if (e.animationName == "animSwipeLeft" || e.animationName == "animSwipeRight" || e.animationName == "animBigFadeInOut") {
        //location.reload()
    }
}

function setVolume(value) {
    ws.control.volume(value);
}

function last() {
    ws.control.last();
}

function playPause() {
    ws.control.playPause();
}

function next() {
    ws.control.next();
}

var gesture = {
        x: [],
        y: [],
        match: ''
    },
    tolerance = 100;
window.addEventListener('touchstart', capture)
window.addEventListener('mousedown', start)
window.addEventListener('touchmove', capture)
window.addEventListener('mousemove', capture)

function start(e) {
    gesture.x = []
    gesture.y = []
}

function capture(e) {
    e.preventDefault()

    try {
        gesture.x.push(e.touches[0].clientX)
        gesture.y.push(e.touches[0].clientY)
    } catch {
        gesture.x.push(e.clientX)
        gesture.y.push(e.clientY)
    }
}
window.addEventListener('touchend', compute)
window.addEventListener('mouseup', compute)

function compute(e) {
    var xStart = gesture.x[0],
        yStart = gesture.y[0],
        xEnd = gesture.x.pop(),
        yEnd = gesture.y.pop(),
        xTravel = xEnd - xStart,
        yTravel = yEnd - yStart;
    if (xTravel < tolerance && xTravel > -tolerance && yTravel < -tolerance) {
        gesture.match = 'Swiped Up'
        value = parseInt(document.getElementById("volumeBar").value)
        document.getElementById("volumeBar").value = value + 10
        setVolume(document.getElementById("volumeBar").value)
    }
    if (xTravel < tolerance && xTravel > -tolerance && yTravel > tolerance) {
        gesture.match = 'Swiped Down'
        document.getElementById("volumeBar").value -= 10
        setVolume(document.getElementById("volumeBar").value)
    }
    if (yTravel < tolerance && yTravel > -tolerance && xTravel < -tolerance) {
        gesture.match = 'Swiped Left'
        next()
        animSwipeL()
    }
    if (yTravel < tolerance && yTravel > -tolerance && xTravel > tolerance) {
        gesture.match = 'Swipe1d Right'
        last()
        animSwipeR()
    }
    if (gesture.match !== '') {
        console.log(gesture.match)
    }
    gesture.x = []
    gesture.y = []
    gesture.match = xTravel = yTravel = ''
}


/************************/

function animPlayPause() {
    document.getElementById("playPause").style.animation = "animBigFadeInOut 0.7s";
    document.getElementById("playPause").style.animationTimingFunction = "linear";

    document.getElementById("playPause").addEventListener('webkitAnimationEnd', function() {
        this.style.animation = null;
        this.style.animationTimingFunction = null;
        this.style.webkitAnimationPlayState = "paused";
    });
}

function animSwipeL() {
    document.getElementById('cover').style.animation = "animSwipeLeft 2s";
    document.getElementById('cover').style.animationFillMode = "forwards";

    document.getElementById("cover").addEventListener('webkitAnimationEnd', function() {
        this.style.animation = null;
        this.style.animationTimingFunction = null;
        this.style.webkitAnimationPlayState = "paused";
    });
}

function animSwipeR() {
    document.getElementById('cover').style.animation = "animSwipeRight 2s";
    document.getElementById('cover').style.animationFillMode = "forwards";

    document.getElementById("cover").addEventListener('webkitAnimationEnd', function() {
        this.style.animation = null;
        this.style.animationTimingFunction = null;
        this.style.webkitAnimationPlayState = "paused";
    });
}

console.log("hello!")