<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');

include($_SERVER['DOCUMENT_ROOT'] . '/products/products.php');

$product = get_product_by_id($_GET["id"]);
$current_page = $product['name'];
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <!-- banner -->
    <?php
    $banner_title = $product['name'];
    $banner_text = $product['desc'];
    $banner_image = $product['banner'];

    include($_SERVER['DOCUMENT_ROOT'] . '/include/banner.php');
    ?>

    <!-- download -->
    <section class="container text-center">
        <?php if ($product['file']) : ?>
            <a href="<?php $product['file'] ?>">
                <img src="/assats/icon/product/download.png" height="150px" class="can-invert">
                <h4>Download Now!</h4>
                <p>
                    <?php echo $product['name'] . ' ' . end($product['patch_note'])['version'] ?>
                </p>
            </a>
        <?php else : ?>
            <h3>Coming soon!</h3>
        <?php endif ?>
    </section>

    <!-- product info -->
    <div class="d-lg-flex container p-0">
        <section class="m-0 mr-lg-4 mb-4 mb-lg-0" style="min-width: auto; min-height: auto;">
            <?php
            if (count($product['features']) > 0) {
                echo '<h4>Features</h4>';
                echo '<div class=" row">';
                foreach ($product['features'] as $item) {
                    echo '<div class="col-md-6">';
                    echo '<h6>' . $item['name'] . '</h6>';
                    echo '<p>' . $item['desc'] . '</p>';
                    echo '</div>';
                }
                echo '</div>';
            }

            if (count($product['requirements']) > 0) {
                echo '<h4>Requirements</h4>';
                echo '<ul>';
                foreach ($product['requirements'] as $item) {
                    echo '<li>' . $item . '</li>';
                }
                echo '</ul>';
            }

            if (count($product['compatibility']) > 0) {
                echo '<h4>Compatibility</h4>';
                echo '<ul>';
                foreach ($product['compatibility'] as $item) {
                    echo '<li>' . $item . '</li>';
                }
                echo '<li style="list-style-type: none;">' . $product['compatibility-note'] . '</li>';
                echo '</ul>';
            }
            ?>
        </section>

        <!-- News and versions -->
        <aside class="m-0" style="min-width: 320px; min-height: auto;">
            <h4>What's New</h4>
            <p>
                <?php echo $product['news']; ?>
            </p>

            <h4>Patch Note</h4>
            <div>
                <?php
                $item_to_display = 5;
                if (count($product['patch_note']) > $item_to_display) {
                    $items = count($product['patch_note']) - $item_to_display - 1;
                } else {
                    $items = -1;
                }

                for ($i = count($product['patch_note']) - 1; $i > $items; $i--) {
                    if ($i > 0) {
                        echo '<hr>';
                    }
                    echo '<h6>' . $product['patch_note'][$i]['version'] . '</h6>';
                    echo '<p>' . $product['patch_note'][$i]['note'] . '</p>';
                }

                ?>
            </div>
        </aside>
    </div>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php'); ?>
</body>

</html>
