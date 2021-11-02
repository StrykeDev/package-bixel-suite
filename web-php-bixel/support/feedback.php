<?php
include($_SERVER['DOCUMENT_ROOT'] . '/config/config.php');
?>

<!DOCTYPE html>
<html lang="en" data-theme="<?php echo $theme ?>">

<?php include($_SERVER['DOCUMENT_ROOT'] . '/include/head.php'); ?>

<body>
    <!-- header and nav -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/header.php'); ?>

    <section class="container">
        <form class="d-none" id="feedback-form">
            <h2>Feedback</h2>
            <p>
                This form is for feedback, suggestion or requests.<br>
                If you have a technical issues with our products please use the support ticket instead.
            </p>

            <p>
                Email and name are optional, if you want to send anonymous feedback just leave them empty.<br>
                Please note that we wont be able to reply to anonymous feedbacks.
            </p>

            <div class="form-group">
                <label for="email">Email address</label>
                <input type="email" name="email" class="form-control" aria-describedby="emailHelp">
                <small id="emailHelp" class="form-text mute">
                    We will never share your email with anyone else.
                </small>
            </div>

            <div class="form-group">
                <label for="name">Name</label>
                <input type="name" name="name" class="form-control">
            </div>

            <div class="form-group">
                <label for="textarea">Message</label>
                <textarea class="form-control" id="textarea" rows="5" required></textarea>
            </div>

            <div class="form-group text-center pt-4">
                <button type="submit" class="btn" style="width:200px;">Submit</button>
            </div>
        </form>

        <div class="text-center">
            <img src="/assats/icon/error/success.png" height="200px" class="can-invert">
            <h2>Thanks for the feedback!</h2>
            <p>We will contact you if necessary.</p>
            <a href="/index.php" class="btn">Go Home</a>
        </div>
    </section>

    <!-- footer -->
    <?php include($_SERVER['DOCUMENT_ROOT'] . '/include/footer.php'); ?>
</body>

</html>
