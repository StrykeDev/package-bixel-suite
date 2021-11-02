<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <!-- banner -->
    <?php
    $banner_title = "Streamline your PC experience";
    $banner_text = "Speed up your workflow with our selection of quality of life solutions.";

    include($_SERVER['DOCUMENT_ROOT'] . '/include/banner.php');
    ?>

    <!-- product catalog -->
    <section class="container">
        <div class="row">
            <?php
            foreach ($items as $item) {
                echo '<a href="/product.php?id=' . $item['id'] . '" class="my-card-large col">';
                echo '<div class="my-card-icon-large">';
                echo '<img src="' . $item['icon'] . '">';
                echo '</div>';
                echo '<h5 class="my-card-title">' . $item['name'] . '<br>' . $item['type'] . '</h5>';
                echo '<p class="my-card-text">' . $item['desc'] . '</p>';
                echo '</a>';
            }
            ?>
        </div>
    </section>

    <!-- donate -->
    <section class="container">
        <h2>Support Us!</h2>
        <p>
            We all love free software but sadly they ain't free to develop and maintain,<br>
            If you have some cash and would like to support us we will appreciate it a lot!<br>
        </p>

        <!-- donation tiers -->
        <div class="row">
            <?php
            $tiers = json_decode(file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/tiers/tiers.json'), true);
            foreach ($tiers as $tier) {
                echo '<div class="col-6 col-md-4 col-lg-2"><a href="/donate.php?amount=' . $tier['amount'] . '" class="my-card">';
                echo '<div class="my-card-icon">';
                echo '<img src="' . $tier['icon'] . '" class="can-invert">';
                echo '</div>';
                echo '<h5 class="my-card-title">$' . $tier['amount'] . '</h5>';
                echo '<p class="my-card-text">' . $tier['desc'] . '</p>';
                echo '</a></div>';
            }
            ?>
    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php');  ?>
</body>

</html>
