window.ws = new function () {
    class Data {
        displayname() {
            webSocket.send(createMessage("data", "displayname"));
        }

        playlists() {
            webSocket.send(createMessage("data", "playlists"));
        }

        playlist(index) {
            webSocket.send(createMessage("data", "playlist", index));
        }

        search(query, scope = "playlist") {
            webSocket.send(createMessage("data", "search", JSON.stringify({
                query,
                scope
            })));
        }

        position() {
            webSocket.send(createMessage("data", "position"));
        }

        volume() {
            webSocket.send(createMessage("data", "volume"));
        }

        cover() {
            webSocket.send(createMessage("data", "cover"));
        }

        radioProgramme() {
            webSocket.send(createMessage("data", "radioProgramme"));
        }

        accentColour() {
            webSocket.send(createMessage("data", "accentColour"));
        }
    }

    class Control {
        next() {
            webSocket.send(createMessage("control", "next"));
        }

        last() {
            webSocket.send(createMessage("control", "last"));
        }

        jump(value) {
            webSocket.send(createMessage("control", "jump", value));
        }

        sort(type) {
            webSocket.send(createMessage("control", "sort", type));
        }

        block(list) {
            webSocket.send(createMessage("control", "block", JSON.stringify(list)));
        }

        playPause() {
            webSocket.send(createMessage("control", "playPause"));
        }

        volume(value) {
            webSocket.send(createMessage("control", "volume", value));
        }

        loadPlaylist(index) {
            webSocket.send(createMessage("control", "load/playlist", index));
        }

        load(index, globally = false) {
            webSocket.send(createMessage("control", "load", JSON.stringify({
                index,
                scope: globally ? "global" : "playlist"
            })));
        }
    }

    class General {
        version(index) {
            webSocket.send(createMessage("general", "version"));
        }
    }

    class Game {
        validateUser(key) {
            webSocket.send(createMessage("game", "validate-user", key));
        }

        library() {
            webSocket.send(createMessage("game", "library"));
        }

        launch(id, user) {
            webSocket.send(createMessage("game", "launch", JSON.stringify({
                id: id,
                user: user
            })));
        }
    }

    class Youtube {
        download(link, output = null) {
            webSocket.send(createMessage("youtube", "download", JSON.stringify({
                link: link,
                output: output
            })));
        }

        sync(link, output = null) {
            webSocket.send(createMessage("youtube", "sync", JSON.stringify({
                link: link,
                output: output
            })));
        }
    }

    let webSocket = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/ws");
    window.wsConnected = false;

    webSocket.onopen = function (event) {
        console.log("[websocket.js] connected to ws");
        window.wsConnected = true;

        console.log(window.onConnected)

        var evt = document.createEvent("Event");
        evt.initEvent("ws.connect", true, true);

        //invoke
        document.dispatchEvent(evt);
    };

    this.data = new Data();
    this.control = new Control();
    this.general = new General();
    this.game = new Game();
    this.youtube = new Youtube();

    webSocket.onmessage = function (event) {
        console.log("[websocket.js]", JSON.parse(event.data));

        const jdata = JSON.parse(event.data)

        var evt = document.createEvent("Event");
        const evtName = "ws." + jdata.apiModule + "." + jdata.endpoint;
        console.info(evtName);
        evt.initEvent(evtName, true, true);

        // custom param
        evt.data = jdata;

        //invoke
        document.dispatchEvent(evt);
    }

    function createMessage(apiModule, endpoint, data = null) {
        const st = JSON.stringify({
            apiModule: apiModule,
            endpoint: endpoint,
            data: data
        });

        console.log("[websocket.js] " + st)

        return st;
    }

}