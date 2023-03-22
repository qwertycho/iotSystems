
getGroups();
setInterval(getGroups, 1000);

async function getGroups() {
fetch("/dashboard/groups")
.then((response) => response.json())
.then((data) => {
  buildDisplay(data)
}) 
}

function buildDisplay(data){
  document.querySelector(".groups").innerHTML = "";
    data.forEach(group => {
       let groupDiv = buildGroup(group)
       document.querySelector(".groups").appendChild(groupDiv);
    });
}

function buildDevice(device){
    let div = document.createElement("div");
    let deviceTitle = document.createElement("h3");
    deviceTitle.innerText = device.name;
    div.appendChild(deviceTitle);
        let sensorDiv = buildSensor(device.sensors);
        div.appendChild(sensorDiv);

    return div;
}

function buildSensor(sensors){
    let div = document.createElement("div");
    let list = document.createElement("ul");
    sensors.forEach(sensor => {
      let item = document.createElement("li");
      item.innerText = sensor;
      list.appendChild(item);
    });
    div.appendChild(list);
    return div;
}

function buildGroup(group){
    let div = document.createElement("div");
    let groupTitle = document.createElement("h2");
    groupTitle.innerText =`Groep: ${group.groupName}`;
    div.appendChild(groupTitle);
    group.devices.forEach(device => {
      div.appendChild(buildDevice(device));
    });
    return div;
}