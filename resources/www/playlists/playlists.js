//register
document.addEventListener("ws.connect", onConnected, false);
document.addEventListener("ws.data.playlists", receivePlaylists, false);

function onConnected() {
    ws.data.playlists();
    console.log("connected to ws!!");
}

function receivePlaylists(evt) {

    const playlists = JSON.parse(evt.data.data)

    console.log(playlists, typeof playlists)

    let section = document.getElementById("cardsection");
    let html = "";
    const maxTagLength = 15;

    for (let i = 0; i < playlists.autoplaylists.length; i++) {
        html += '<article class="card"><header class="card-header"><p>' + playlists.autoplaylists[i].date +
            '</p><h2>★ ' + playlists.autoplaylists[i].name + '</h2><p>' + playlists.autoplaylists[i]
            .description +
            '</p></header><div class="card-author"><div class="author-name">Automatically Created</div></div><div class="tags">';

        for (let j = 0; j < playlists.autoplaylists[i].tags.length; j++) {
            let tag = playlists.autoplaylists[i].tags[j];
            if (tag.length > maxTagLength) {
                tag = tag.substring(0, maxTagLength - 3) + "...";
            }

            html += '<a href="#">' + tag + '</a>';
        }

        html += '</div></article>';
    }

    for (let i = 0; i < playlists.customplaylists.length; i++) {
        html += '<article class="card"><header class="card-header"><p>' + playlists.customplaylists[i].date +
            '</p><h2 onclick="ws.control.loadPlaylist(' + i + ')">' + playlists.customplaylists[i].name + '</h2><p>' + playlists.customplaylists[i]
            .description +
            '</p></header><div class="card-author"><div class="author-name">Handpicked</div></div><div class="tags">';

        for (let j = 0; j < playlists.customplaylists[i].tags.length; j++) {
            let tag = playlists.customplaylists[i].tags[j];
            if (tag.length > maxTagLength) {
                tag = tag.substring(0, maxTagLength - 3) + "...";
            }

            html += '<a href="#">' + tag + '</a>';
        }

        html += '</div></article>';
    }

    section.innerHTML = html;
}