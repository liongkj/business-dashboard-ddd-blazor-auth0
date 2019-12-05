let map ;
let latField = document.getElementById("latValue");
let lngField = document.getElementById("lngValue");
const DefaultCoordinates = {lat:2.7119454,lng:101.812422};
let mapOptions = {
    zoom: 8,
    center: DefaultCoordinates,
    mapTypeControl:false,
};


$(function(){
    $('#modal-address').on('shown.bs.modal',function (){
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

function loadMap(){
    let lat = parseFloat(latField.value);
    let lng = parseFloat(lngField.value);

    if(lat && lng){
        var newLocation = {lat:lat,lng:lng};
        addMarker(newLocation);
        animateTo(newLocation);
    }
}

function animateTo(location){
    if(location!== DefaultCoordinates) {
        map.setCenter(location);
        map.setZoom(16);
    }
}

function addMarker(newLocation) {
    // Add the marker at the clicked location, and add the next-available label
    // from the array of alphabetical characters.
    var marker = new google.maps.Marker({
        draggable:true,
        animation: google.maps.Animation.DROP,
        position:newLocation ,
        map: map
    });


    function markerCoords(markerobject){
        google.maps.event.addListener(markerobject, 'dragend', function(evt){
            infoWindow.setOptions({
                content: '<p>Marker dropped: Current Lat: ' + evt.latLng.lat().toFixed(3) + ' Current Lng: ' + evt.latLng.lng().toFixed(3) + '</p>'
            });
            infoWindow.open(map, markerobject);
        });

        google.maps.event.addListener(markerobject, 'drag', function(evt){
            console.log("marker is being dragged");
        });
    }
}
// google.maps.event.addDomListener(window, 'load', initialise);
      
      