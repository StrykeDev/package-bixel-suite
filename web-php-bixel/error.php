<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');

$error = get_error_by_code(http_response_code());
$current_page = http_response_code();

function get_error_by_code($code)
{
    if ($code >= 200 && $code < 300)
        $code = 200;

    if ($code >= 500 && $code < 600)
        $code = 500;

    $errors = json_decode(file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/error/errors.json'), true);
    foreach ($errors as $error)
        if ($error['code'] == $code)
            return $error;

    return $errors[0];
}
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <!-- banner -->
    <?php
    $banner_title = http_response_code();
    $banner_text = $error['desc'];
    $banner_image = "/assats/banner/support_banner.png";

    include($_SERVER['DOCUMENT_ROOT'] . '/include/banner.php');
    ?>

    <!-- go home -->
    <section class="container text-center">
        <img src="/assats/icon/error/warning.png" width="200px" class="can-invert">
        <h4>¯\_(ツ)_/¯</h4>
        <p>Well there's nothing here...</p>
        <a href="/index.php" class="btn">Go Home</a>
    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php');  ?>
</body>

</html>
