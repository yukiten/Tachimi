'use strict';

const messagesList = document.getElementById('messagesList');
const userInput = document.getElementById('userInput');
const messageInput = document.getElementById('messageInput');
const connections = document.getElementById('connections');

if (!messagesList || !userInput || !messageInput || !connections) {
    console.error('Error: One or more required DOM elements not found.');
}

const connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    var roomName = document.getElementById("roomName").value;
    connection.invoke("AddToGroup", roomName).catch(function (err) {
        return console.error(err.toString());
    });
});

document.getElementById('sendButton').addEventListener('click', (event) => {
    event.preventDefault();

    const user = userInput.value;
    const message = messageInput.value;
    var roomName = document.getElementById("roomName").value;

    if (!user || !message) {
        alert('Please enter a valid username and message.');
        return;
    }

    messageInput.value = '';

    connection.invoke('SendMessage', roomName, user, message)
        .catch((err) => console.error(`Error sending message: ${err.toString()}`));
});

connection.on('ReceiveMessage', (user, message) => {
    const li = document.createElement('li');
    li.textContent = `${user}: ${message}`;
    messagesList.appendChild(li);
    messagesList.scrollTop = messagesList.scrollHeight;
});

connection.on('updateConnections', (numConnections) => {
    connections.textContent = numConnections;
});
