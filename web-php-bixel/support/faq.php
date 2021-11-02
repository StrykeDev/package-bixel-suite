<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');

$faq = get_data_by_id($_GET['id']);
$current_page = $faq['product_name'] . " F.A.Q";

function get_data_by_id($id)
{
    $faq_list = json_decode(file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/support/faq.json'), true);

    foreach ($faq_list as $item) {
        if ($item['product_id'] == $id) {
            return $item;
        }
    }

    header("Location: /support-center.php");
}
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <section class="container">
        <h2><?php echo $faq['product_name'] . ' F.A.Q' ?></h2>
        <p>
            Can't find the answer you were looking for? In that case feel free to <a href="/support-center.php">contact us</a>.
        </p>

        <ul>
            <?php
            foreach ($faq['faq'] as $key => $value) {
                echo '<li><h5><a href="#faq-' . $key . '" data-toggle="collapse" data-target="#faq-' . $key . '">';
                echo $value['question'];
                echo '</a></h5><div id="faq-' . $key . '" class="collapse"><p>';
                echo $value['answer'];
                echo '</p>';
                if ($value['dxdiag']) {
                    echo '<p>How to get a <b>DXDIAG</b> file:<br> Click on the <b>Start</b> button and type <b>dxdiag</b> into the <b>search box</b> then select <b>dxdiag</b> from the results.<br>In the tool, click on <b>Save All Information...</b> and save the dxdiag file on your desktop.</p>';
                }
                echo '</div></li><hr class="mr-5">';
            }
            ?>
        </ul>

    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php');  ?>
</body>

</html>
