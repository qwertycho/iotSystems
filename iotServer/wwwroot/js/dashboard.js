
getGroups();
setInterval(getGroups, 1000);

async function getGroups() {
fetch("/dashboard/groups")
.then((response) => response.json())
.then((data) => {
  buildDisplay(data)
}) 
}

/**
 * 
 * @param {*} group json array
 * Loops through all the groups and calls buildGroup for each group.
 * Clears the groups div and appends the new groups.
 */
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

/**
 * 
 * @param {group} group
 * @returns group div
 * Builds a group div that contains all the devices in the group.
 * Loops through all the devices in the group and calls buildDevice for each device.
 */
function buildGroup(group){
    let div = document.createElement("div");
    let groupTitle = document.createElement("h2");
    groupTitle.innerText =`Groep: ${group.name}`;
    div.appendChild(groupTitle);
    group.devices.forEach(device => {
      div.appendChild(buildDevice(device));
    });
    return div;
}