<footer class="shadow-box">
    <div class="container mute">
        <img src="/assats/brand/logo_bixel_g_x256.png" height="32px">
        <p>Copyright &copy; <?php echo date("Y"); ?> - All rights reserved.</p>

        <ul class="d-flex" style="list-style:none; padding: 0;">
            <li class="mr-4"><a href="/legal/tos.php">Terms of Service</a></li>
            <li class="mr-4"><a href="/legal/privacy.php">Privacy Policy</a></li>
            <li class="mr-4"><a href="/legal/cookies.php">Cookies Policy</a></li>
            <li class="mr-4"><a href="" data-toggle="modal" data-target="#credit">Credit</a></li>
        </ul>
    </div>
</footer>

<!-- credit -->
<style>
    .modal-content {
        background: var(--color-bg) !important;
        padding: 20px;
        border-bottom: solid var(--color-highlight) 2px;
    }
</style>
<div class="modal fade" id="credit" tabindex="-1" role="dialog" aria-labelledby="credit" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
            <h4>Thank you!</h4>

            <ul>
                <?php
                $credit = json_decode(file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/credit/credit.json'), true);

                foreach ($credit as $value) {
                    echo '<li><a href="' . $value['resource-url'] . '" target="_blank">' . $value['resource'] . '</a> ' . $value['type'] . ' made by <a href="' . $value['author-url'] . '" target="_blank">' . $value['author'] . '</a>.</li>';
                }
                ?>
            </ul>

            <p class="mute">All of the resources mentioned above are licensed by <a href="http://creativecommons.org/licenses/by/3.0/" target="_blank">CC 3.0 BY</a>.</p>

            <div class="text-center">
                <button class="btn" data-toggle="modal" data-target="#credit">Close</button>
            </div>
        </div>
    </div>
</div>