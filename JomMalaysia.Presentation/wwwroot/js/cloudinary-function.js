﻿cloudinary.setCloudName("jomn9-com");

var logoWidget;
var coverWidget;
var adsWidget;
$(function () {
        logoWidget = cloudinary.createUploadWidget(
            {
                cloudName: 'jomn9-com',
                uploadPreset: 'ox3azzqe',
                apiKey: '731121621859821',
                folder: 'listing_image',
                sources: ["local", "image_search"],
                googleApiKey: 'AIzaSyCZseAlp_Rd9CxaJtOQBVKvpkBl8gHiXvk',
                searchBySites: ["all", "unsplash.com", "freepik.com"],
                croppingAspectRatio: 1,
                cropping: "server",
                croppingShowDimensions: true,
                multiple: false,
                showCompleted: true,
                croppingShowBackButton: true,
                resourceType: ["image"],
                clientAllowedFormats: ["png", "jpg", "jpeg"],
                minImageWidth: 100,
                minImageHeight: 100,
                showSkipCropButton: true,
                thumbnails: ".image-thumbnail.listing-logo",
                thumbnailTransformation: "w_150,h_150,c_fill"
            },(error, result) => {
                if (!error) {

                }

                if (result.event === "upload-added") {
                    setLoading(true);
                }
                if (!error && result && result.event === "success") {
                    setLoading(false);

                    $(".imageUrl.listing-logo").val(result.info.secure_url);
                    $(".imageName.listing-logo").val(result.info.original_filename + "." + result.info.format);

                }
            }
        );
    console.log(logoWidget);
    //cover
        coverWidget = cloudinary.createUploadWidget(
            {
                cloudName: 'jomn9-com',
                uploadPreset: 'ox3azzqe',   
                apiKey: '731121621859821',
                folder: 'listing_image',
                sources: ["local", "image_search"],
                googleApiKey: 'AIzaSyCZseAlp_Rd9CxaJtOQBVKvpkBl8gHiXvk',
                searchBySites: ["all", "unsplash.com", "freepik.com"],
                croppingAspectRatio: 1.3,
                cropping: "server",
                croppingShowDimensions: true,
                multiple: false,
                showCompleted: true,
                croppingShowBackButton: true,
                resourceType: ["image"],
                clientAllowedFormats: ["png", "jpg", "jpeg"],
                minImageWidth: 100,
                minImageHeight: 100,
                maxFiles: 5,
                showSkipCropButton: true,
                thumbnails: ".image-thumbnail.listing-cover",
                thumbnailTransformation: "w_150,h_150,c_fill"
            },(error, result) => {
                if (!error) {
                    
                }

                if (result.event === "upload-added") {
                    setLoading(true);
                }
                if (!error && result && result.event === "success") {
                    setLoading(false);
                   
                        $(".imageUrl.listing-cover").val(result.info.secure_url);
                        $(".imageName.listing-cover").val(result.info.original_filename + "." + result.info.format);
                    
                }
            }
        );
       
    
}
);

function uploadLogoWidget() {
    logoWidget.open();
}


function uploadCoverWidget() {
    coverWidget.open();
}

function uploadAdsWidget(no) {
    
    //ads
    adsWidget = cloudinary.openUploadWidget(
        {
            cloudName: 'jomn9-com',
            uploadPreset: 'ox3azzqe',
            apiKey: '731121621859821',
            folder: 'listing_image',
            sources: ["local", "image_search"],
            googleApiKey: 'AIzaSyCZseAlp_Rd9CxaJtOQBVKvpkBl8gHiXvk',
            searchBySites: ["all", "unsplash.com", "freepik.com"],
            croppingShowDimensions: true,
            multiple: true,
            cropping: false,
            croppingAspectRatio: 0.6,
            showCompleted: true,
            croppingShowBackButton: true,
            resourceType: ["image"],
            clientAllowedFormats: ["png", "jpg", "jpeg"],
            minImageWidth: 100,
            minImageHeight: 100,
            maxFiles: 5,
            showSkipCropButton: true,
            thumbnailTransformation: "w_150,h_150,c_fill"
        },(error, result) => {
            if (!error) {

            }

            if (result.event === "upload-added") {
                setLoading(true);
            }
            if (!error && result && result.event === "success") {
                setLoading(false);
                $("#open_pre_widget1").on("click", function() {
                    preWidget.open(null, {files: [result.info.secure_url]});
                });


            }
        }
    );

    $("#open_pre_widget1").on("click", function() {
        preWidget.open(null, {files: [result.info.secure_url]});
    });
}

function parseAdsImage() {
    let adList = $(".image-thumbnail.listing-ads ul").children();
    let adCount = 0;
    adList.each(function () {
        let url = $(this).data("cloudinary").secure_url;
        $(".ads-group").append('<input name="ListingImages.Ads[' + adCount + '].Url" type="hidden" ' +
            'value="' + url + '">');
        adCount += 1;
    });


}