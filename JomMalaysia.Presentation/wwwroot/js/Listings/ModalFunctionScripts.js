let validator;
let to_validate_fields;
//init validator
$(function () {

    validator = $("form").validate();
    //populate category based on type
    $("#type-input").change(function() {
        let filterValue = $("#type-input option:selected").text().toLowerCase();
        
        let categoryDropdown = $("#category-dropdown");
        categoryDropdown.prop('disabled', true);
        if(filterValue!=null){

            $.ajax({
                type: "GET",
                url: "/Category/GetCategoryByType",
                data: {type:filterValue},
                complete: function (data) {
                    let result =data.responseJSON;
                    categoryDropdown.empty();
                    if(result.length === 0){
                        categoryDropdown.append($('<option>', {
                            value: -1,
                            text: '--- This type do not have category yet ---',
                            selected: true
                        }));
                    }else {
                        let $prevGroup, prevGroupName;
                        $.each(result, function () {
                            debugger
                            categoryDropdown.prop("disabled",false);
                            if (prevGroupName !== this.group.name) {
                                $prevGroup = $('<optgroup />').prop('label', this.group.name).appendTo('#category-dropdown');
                            }
                            $("<option />").val(this.value).text(this.text).appendTo($prevGroup);
                            prevGroupName = this.group.name;
                        });
                    }

                },
            });
        }
    });

});

// validte field function
function validate(fields) {
    let isValid = validator.element(fields);
    debugger
    if (isValid) {
        to_validate_fields += 1;
    }
}

// save info on contact modal
function saveContact() {
    to_validate_fields = 0;
    let fields = document.querySelectorAll(".contact_field");
    fields.forEach(validate);
    
    if (to_validate_fields === fields.length) {
        $("#modal-contact").hide();
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    }
}

function saveAddress() {
    to_validate_fields = 0;
    let fields = document.querySelectorAll(".add_field");
    fields.forEach(validate);
    
    if (to_validate_fields === fields.length) {
        $("#modal-address").hide();
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    }
}

//toggle checkbox show hour field
function showHourField(cb) {
    let day = $(cb).data('day');
    let field = $("#hour-field-" + day);
    let label = $($(cb)[0].labels[0]);

    if (cb.checked) {
        field.css("display", "block");
        label.text("Open");
    } else {
        field.css("display", "none");
        label.text("Closed");
    }
}

function saveOperating() {
    var UpdatedHours = [];
    let fields=[];
    to_validate_fields = 0;
    for (var i = 0; i < 7; i++) {

        let enabled = $("#OperatingHours_" + i + "__Enabled");
        let checked = enabled[0].checked;

        if (checked) {
            let start = $("#OperatingHours_" + i + "__OpenTime");
            let close = $("#OperatingHours_" + i + "__CloseTime");
            fields.push(start);
            fields.push(close);
            
        }
        let UpdatedDay = {
            day: i,
            enabled: checked,
        };
        UpdatedHours.push(UpdatedDay);
    }
    
    fields.forEach(validate);
    if (to_validate_fields === fields.length) {
        
        if (UpdatedHours.length > 0) {
            updateFormHours(UpdatedHours);
        }
      
        $("#modal-operating").hide();
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    }
}

function updateFormHours(UpdatedHours) {

    var htmlstring;
    UpdatedHours.forEach(function({day,enabled}){
        let NChild = day+1;
        let status = enabled?'Open':'Closed';
        htmlstring =
            '<label type="text" class="label">' +
            status +
            '</label>';
        let toUpdate = $("#business-hours-list .row:nth-child(" + NChild + ") div:nth-child(2)");
        toUpdate.empty().html(htmlstring);
        
    });


}