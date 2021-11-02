<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');

$faq_list = json_decode(file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/support/faq.json'), true);
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <!-- banner -->
    <?php
    $banner_title = "Support Center";
    $banner_text = "How can we help you?";
    $banner_image = "/assats/banner/support_banner.png";

    include($_SERVER['DOCUMENT_ROOT'] . '/include/banner.php');
    ?>

    <section class="container">
        <div class="row">

            <!-- faq -->
            <section class="col-lg-4  mb-5">
                <h4>F.A.Q</h4>
                <p>
                    Check out those frequently asked questions.<br>
                    It might solve your issue.
                </p>
                <h6>Software</h6>
                <ul>
                    <?php
                    foreach ($faq_list as $key => $value) {
                        echo '<a href="support/faq.php?id=' . $value['product_id'] . '"><li>' . $value['product_name'] . '</li></a>';
                    }
                    ?>
                </ul>
            </section>


            <div class="row col-lg-8">

                <!-- support ticket -->
                <div class="col-lg-6 mb-5 order-lg-0">
                    <h4>Support Ticket</h4>
                    <p>
                        Support tickets takes time, we recommend that you check the FAQ first.
                    </p>
                    <a class="btn" href="/support/ticket.php">Contact Us</a>
                </div>

                <!-- feedback -->
                <div class="col-lg-6 mb-5 order-lg-2">
                    <h4>Feedback</h4>
                    <p>
                        Feel free to give us feedback, suggestion or requests for currect and future products.
                    </p>
                    <a class="btn" href="/support/feedback.php">Send Feedback</a>
                </div>

                <!-- business -->
                <div class="col-lg-6 mb-5 order-lg-1">
                    <h4>Business</h4>
                    <p>
                        This channel is for legal and businesses.<br>
                        Support tickets will be filtered out.
                    </p>
                    <a class="btn" href="/support/contact-us.php">Contact Us</a>
                </div>

                <!-- policies -->
                <div class="col-lg-6 mb-5 order-lg-3">
                    <h4>Our Policies</h4>
                    <ul>
                        <li class="mr-4"><a href="/legal/tos.php">Terms of Service</a></li>
                        <li class="mr-4"><a href="/legal/privacy.php">Privacy Policy</a></li>
                        <li class="mr-4"><a href="/legal/cookies.php">Cookies Policy</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php'); ?>
</body>

</html>
