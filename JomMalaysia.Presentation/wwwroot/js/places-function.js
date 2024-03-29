﻿
let nameField = $("#listing-name-autocomplete");
let autofillField = $("#autofill");

autofillField.click(function(){
    if(autofillField.is(":checked")&& nameField.val()) {
        AutofillAddress();
    }
});


nameField.focusout(function(){
    
    if(autofillField.is(":checked") && nameField.val()) {
        AutofillAddress();
    }
});

function AutofillAddress() {
    let service = new google.maps.places.PlacesService(document.getElementById('service-helper'));

    let locationBias = google.maps.LatLngBounds(google.maps.LatLng(100.085756871, 0.773131415201), google.maps.LatLng(119.181903925, 6.92805288332));
    let request = {
        query: nameField.val(),
        locationBias: locationBias,
        fields: ['name', 'geometry','formatted_address','place_id'],
    };


    service.findPlaceFromQuery(request, function (basicInfo, status) {

        if (status === google.maps.places.PlacesServiceStatus.OK) {
            nameField.val(basicInfo[0].name);
            var detailRequest = {
                placeId : basicInfo[0].place_id,
                fields: ['address_component',"geometry"],
            };
            $("#txtFullAddress").val(basicInfo[0].formatted_address);

            service.getDetails(detailRequest,function (detail,status){
                if (status === google.maps.places.PlacesServiceStatus.OK) {
                    let address = detail.address_components;
                    $("#Address_Add1").val(GetAddressPart(address,"add1"));
                    $("#Address_Add2").val(GetAddressPart(address,"add2"));
                    $("#Address_City").val(GetAddressPart(address,"city"));
                    $("#Address_PostalCode").val(GetAddressPart(address,"postal"));
                    $("#Address_State").val(GetAddressPart(address,"state"));
                    $("#Address_Country").val(GetAddressPart(address,"country"));

                    let coordinates = detail.geometry.location;
                    $("#Address_Coordinates_Latitude").val(coordinates.lat);
                    $("#Address_Coordinates_Longitude").val(coordinates.lng);
                    let viewport = detail.geometry.viewport;
                    autofillField.prop("checked", false);
                    
                }
                else{
                    alert("fail loading address");
                }}
            )
        }
        else{
            alert("Listing not found")
        }
    });
}

function GetAddressPart(address,parts){
    if(parts === "add1"){
        let premise = MapAddress(address,"premise");
        let subpremise = MapAddress(address,"subpremise");
        let streetNo = MapAddress(address,"street_number");
        let street = MapAddress(address,"route");
        return [premise, subpremise, streetNo, street].filter(Boolean).join(" ");
    }
    if(parts === "add2"){
        return MapAddress(address,"sublocality_level_1");

    }
    if(parts === "city"){
        return MapAddress(address,"locality");
    }
    if(parts === "country"){
        return MapAddress(address,parts,true);
    }
    if(parts==="state"){
        let stateName = MapAddress(address,"administrative_area_level_1").toLowerCase();
        let enumList = JSON.parse($("#StateList").val());
        return enumList[stateName];

    }

    if(parts==="postal"){
        return MapAddress(address,"postal_code");
    }
    return "";
}

function MapAddress(address,parts,getShort){
    if(getShort){
        return address.filter(add=>add.types[0] === parts).map(add=>add.short_name)[0];
    }
    return address.filter(add=>add.types[0] === parts).map(add=>add.long_name)[0];
}
