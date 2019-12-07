let validator;
let to_validate_fields;
$(function () {

    validator = $("form").validate();
    
});

function validate(fields){
    if(validator.element(fields)){
        to_validate_fields+=1;
    }
}

function saveContact(){
    to_validate_fields = 0;
    let fields = document.querySelectorAll(".contact_field");

    fields.forEach(validate);
    if(to_validate_fields === fields.length){
        $("modalContact").hide();
    }
}

function saveAddress(){
    to_validate_fields = 0;
    let fields = document.querySelectorAll(".add_field");
    
    fields.forEach(validate);
    if(to_validate_fields === fields.length){
        $("modalAddress").hide();
    }
}

//toggle checkbox show hour field
function showHourField(checkbox) {

    let day = $(checkbox).data('day');

    if (checkbox.checked) {
        $("#hour-field-" + day).css("display", "block");

    } else {
        $("#hour-field-" + day).css("display", "none");
    }
}   
