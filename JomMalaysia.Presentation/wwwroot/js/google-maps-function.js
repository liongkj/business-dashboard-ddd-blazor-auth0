let map;
let latField = document.getElementById("Address_Coordinates_Latitude");
let lngField = document.getElementById("Address_Coordinates_Longitude");
const DefaultCoordinates = {lat: 2.7119454, lng: 101.812422};
var marker;
let mapOptions = {
    zoom: 8,
    center: DefaultCoordinates,
    mapTypeControl: false,
};


$(function () {
    $('#modal-address').on('shown.bs.modal', function () {
        initialise();
        loadMap();
    });
});

function initialise() {

    console.log("initialising map");

    map = new google.maps.Map(document.getElementById('listingMap'),
        mapOptions);

    addMarker(DefaultCoordinates);
    animateTo(DefaultCoordinates);

}

function loadMap() {
    let lat = parseFloat(latField.value);
    let lng = parseFloat(lngField.value);

    if (lat && lng) {
        var newLocation = {lat: lat, lng: lng};
        addMarker(newLocation);
        animateTo(newLocation);
    }
}

function animateTo(location) {
    if (location !== DefaultCoordinates) {
        map.setCenter(location);
        map.setZoom(16);
    }
}

function addMarker(newLocation) {
    // Add the marker at the clicked location, and add the next-available label
    // from the array of alphabetical characters.
    marker = new google.maps.Marker({
        draggable: true,
        animation: google.maps.Animation.DROP,
        position: newLocation,
        map: map
    });


    var positionStart, positionStartNew;
    google.maps.event.addListener(marker, 'dragstart', function () {
        positionStart = this.position;
    });

    google.maps.event.addListener(marker, 'dragend', function () {
        positionStartNew = this.position.toJSON();
        latField.value = positionStartNew.lat;
        lngField.value = positionStartNew.lng;
    });
}

      
      