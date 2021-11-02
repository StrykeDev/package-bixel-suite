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
    $banner_title = "help Needed!";
    $banner_text = "With your support we will be able to develop more free software.";

    include($_SERVER['DOCUMENT_ROOT'] . '/include/banner.php');
    ?>

    <!-- donation form -->
    <style>
        input[type=number]::-webkit-inner-spin-button,
        input[type=number]::-webkit-outer-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>

    <section class="container text-center">
        <div id="donation">
            <form id="donate" action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_blank">
                <img src="/assats/icon/donate/panda_sad.png" width="200px">
                <h4>Feel free to donate as much as you like.</h4>

                <input type="hidden" name="cmd" value="_donations" />
                <input type="hidden" name="business" value="attias.barak@gmail.com" />
                <input type="hidden" name="item_name" value="Software Development" />
                <input type="hidden" name="currency_code" value="USD" />
                <span class="btn-group m-2">
                    <span class="btn btn-active">USD</span>
                    <input class="btn btn-text" type="number" name="amount" min="1" max="100000000" value="<?php echo (isset($_GET['amount']) && $_GET['amount'] > 0 ? $_GET['amount'] : 2) ?>" style="width:150px;" required>
                    <button class="btn" type="submit">Donate</button>
                </span>
            </form>
            <p>
                The transaction process will be handled securely via <b>PayPal</b>.<br>
            </p>
            <p id="dollar-note" style="display: none;">
                <b>Please note:</b> PayPal will take about 35% fee from $1 donations.<br>If you can please donate $2 (or more) instead.
            </p>
        </div>

        <div id="donation-ty" class="d-none">
            <img src="/assats/icon/donate/panda.png" width="200px">
            <h4>Wow thanks!</h4>
            <p>
                Please complete the transaction in the new tab.
            </p>
        </div>

        <script>
            $(document).ready(function() {
                var tbxAmount = $("input[name='amount']", '#donate');
                if (tbxAmount.val() == 1) {
                    $('#dollar-note').fadeIn();
                }

                tbxAmount.change(oneDollarMsg);
                tbxAmount.on('change keyup paste', oneDollarMsg);

                function oneDollarMsg() {
                    if (tbxAmount.val() == 1) {
                        $('#dollar-note').fadeIn();
                    } else {
                        $('#dollar-note').fadeOut();
                    }
                };

                $('#donate').submit(function(e) {
                    e.preventDefault;

                    document.getElementById('donation').classList.add('d-none');
                    document.getElementById('donation-ty').classList.remove('d-none');
                });
            })
        </script>
    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php'); ?>
</body>

</html>
