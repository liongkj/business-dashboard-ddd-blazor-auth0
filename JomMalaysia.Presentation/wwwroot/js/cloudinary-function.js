cloudinary.setCloudName("jomn9-com");

var widget;
$(function () {
    widget = cloudinary.createUploadWidget(
        {
            cloudName: 'jomn9-com',
            uploadPreset: 'ox3azzqe',
            apiKey: '731121621859821',
            folder: 'listing_image',
            sources: ["local", "image_search"],
            googleApiKey: 'AIzaSyCZseAlp_Rd9CxaJtOQBVKvpkBl8gHiXvk',
            searchBySites: ["all", "unsplash.com", "freepik.com"],
            croppingAspectRatio: 0.6,
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
        }
    )
});

function uploadLogoWidget(folder, imageType) {
    
    let field = imageType;
    let isLogo = field.indexOf("logo") >= 0;
    let isCover = field.indexOf("cover") >= 0;
    let isAds = field.indexOf("ads") >= 0;
    let thumb = ".image-thumbnail." + field;
    debugger
    widget.update({
            folder: folder,
            croppingAspectRatio: 1,
            thumbnails: thumb,
        }, (error, result) => {
            if (!error) {
                debugger
            }

            if (result.event === "upload-added") {
                setLoading(true);
            }
            if (!error && result && result.event === "success") {
                setLoading(false);
                if (!isAds) {
                    $(".imageUrl." + field).val(result.info.secure_url);
                    $(".imageName." + field).val(result.info.original_filename + "." + result.info.format);
                }
            }
        }
    );
    widget.open();

}

function uploadCoverWidget(folder, imageType) {
    
    let field = imageType;
    let isLogo = field.indexOf("logo") >= 0;
    let isCover = field.indexOf("cover") >= 0;
    let isAds = field.indexOf("ads") >= 0;
    console.log(".image-thumbnail.listing-cover");
    debugger
    widget.update({
        folder: folder,
        croppingAspectRatio: 1.3,
        thumbnails: ".image-thumbnail .listing-cover",
    }, (error, result) => {
        if (!error) {

        }

        if (result.event === "upload-added") {
            setLoading(true);
        }
        if (!error && result && result.event === "success") {
            setLoading(false);
            if (!isAds) {
                $(".imageUrl." + field).val(result.info.secure_url);
                $(".imageName." + field).val(result.info.original_filename + "." + result.info.format);
            }
        }
    });
    widget.open();
}

function uploadAdsWidget(folder, imageType) {
    
    let field = imageType;
    let isLogo = field.indexOf("logo") >= 0;
    let isCover = field.indexOf("cover") >= 0;
    let isAds = field.indexOf("ads") >= 0;
    debugger
    widget.update(
        {
            folder: folder,
            multiple: true,
            cropping: false,
            croppingAspectRatio: 0.6,
            thumbnails: ".image-thumbnail." + field,
        }, (error, result) => {
            if (!error) {

            }

            if (result.event === "upload-added") {
                setLoading(true);
            }
            if (!error && result && result.event === "success") {
                setLoading(false);
                if (!isAds) {
                    $(".imageUrl." + field).val(result.info.secure_url);
                    $(".imageName." + field).val(result.info.original_filename + "." + result.info.format);
                }
            }
        }
    );
    widget.open();
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