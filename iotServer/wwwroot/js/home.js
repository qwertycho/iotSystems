let sock = new WebSocket("ws://localhost:5001/home/ws")
sock.onopen = function (event) {
    console.log("Connection established");
    sock.send("Hello from client");
    console.log("Message sent");
}


sock.onmessage = function (event) {
    console.log(event.data);
}

sock.onclose = function (event) {
    if (event.wasClean) {
        console.log(`Connection closed cleanly, code=${event.code} reason=${event.reason}`);
    } else {
        console.log('Connection died');
    }
}

sock.onerror = function (error) {
    console.log(`Error: ${error.message}`);
}

document.getElementById("myButton").addEventListener("click", function () {
    sock.send("ding: 1");
    console.log("Message sent");
});