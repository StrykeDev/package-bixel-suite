<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <!-- cookies policy -->
    <section class="container">
        <h4>Cookies</h4>
        <p>
            Cookies are files with small amount of data that is commonly used an anonymous unique
            identifier.<br>
            These are sent to your browser from the website that you visit and are stored on your computer's
            hard drive.
        </p>
        <p>
            Our website uses these cookies to collection information and to improve our services.<br>
            You have the option to either accept or refuse these cookies, and know when a cookie is being sent
            to your
            computer.<br>
            If you choose to refuse our cookies, you may not be able to use some portions of our
            services.
        </p>

        <div class="d-flex">
            <img src="/assats/icon/cookies/cookie.png" height="100px" class="can-invert ml-auto">
        </div>
    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php'); ?>
</body>

</html>
