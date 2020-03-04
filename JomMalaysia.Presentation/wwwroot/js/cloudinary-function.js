cloudinary.setCloudName("jomn9-com");

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
                autoMinimize:true,
                resourceType: ["image"],
                clientAllowedFormats: ["png", "jpg", "jpeg"],
                minImageWidth: 100,
                minImageHeight: 100,
                showSkipCropButton: true,
            },(error, result) => {
                if (!error) {

                }

                if (result.event === "upload-added") {
                    setLoading(true);
                }
                if (!error && result && result.event === "success") {
                    setLoading(false);
                    
                    $(".imageUrl.listing-logo").val(result.info.secure_url);
                    
                    $(".img-thumbnail.listing-logo").attr("src",result.info.secure_url);
                    debugger
                }
            }
        );
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
                croppingShowBackButton: true,
                resourceType: ["image"],
                clientAllowedFormats: ["png", "jpg", "jpeg"],
                minImageWidth: 100,
                minImageHeight: 100,
                showSkipCropButton: true,
            },(error, result) => {
                if (!error) {
                    
                }

                if (result.event === "upload-added") {
                    setLoading(true);
                }
                if (!error && result && result.event === "success") {
                    setLoading(false);
                   
                        $(".imageUrl.listing-cover").val(result.info.secure_url);
                        $(".img-thumbnail.listing-cover").attr("src",result.info.secure_url);
                    
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
            cropping: "server",
            showCompleted: true,
            croppingShowBackButton: true,
            resourceType: ["image"],
            clientAllowedFormats: ["png", "jpg", "jpeg"],
            minImageWidth: 512,
            minImageHeight: 300,
            showSkipCropButton: true,
        },(error, result) => {
            if (result.event === "upload-added") {
                setLoading(true);
            }
            if (!error && result && result.event === "success") {
                setLoading(false);
                $(".ads-thumbnail.listing-ads-"+no).attr("src",result.info.secure_url);
                $(".image-url.listing-ads-"+no).val(result.info.secure_url);
            }
        }
    );

  
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