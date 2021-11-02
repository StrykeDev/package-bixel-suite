<?php
if (!isset($banner_title)) {
    $banner_title = "";
}

if (!isset($banner_text)) {
    $banner_text = "";
}

if (!isset($banner_image)) {
    $banner_image = "/assats/banner/default_banner.png";
}
?>

<style>
    .banner-title {
        color: rgb(0, 0, 0) !important;
    }

    .banner-text {
        color: rgb(0, 0, 0) !important;
    }

    #banner {
        background: rgb(255, 255, 255) !important;
        padding: 0 !important;
        margin: 0 0 40px 0 !important;
        height: 250px;
        width: 100%;
    }

    #banner-image {
        background: url('<?php echo $banner_image ?>') no-repeat;
        background-position: center;
        background-size: cover;
        height: 100%;
    }

    #banner-inner {
        width: 50%;
        padding: 40px 0;
    }
</style>

<section id="banner">
    <div id="banner-image" class="container">
        <div id="banner-inner">

            <h2 class="banner-title">
                <?php echo $banner_title ?>
            </h2>

            <p class="banner-text">
                <?php echo $banner_text ?>
            </p>
        </div>
    </div>
</section>