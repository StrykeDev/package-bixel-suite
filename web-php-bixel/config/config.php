<?php
// Theme
if (isset($_COOKIE['theme'])) {
    if ($_COOKIE['theme'] == 'light') {
        $theme = "light";
    } else {
        $theme = "dark";
    }
} else {
    $theme = "dark";
}

switch ($_SERVER['PHP_SELF']) {
    default:
        $current_page = "Home";
        break;

        // Support center
    case "/support-center.php":
        $current_page = "Support Center";
        break;

    case "/support/feedback.php":
        $current_page = "Feedback";
        break;

    case "/support/ticket.php":
        $current_page = "Support Ticket";
        break;

    case "/support/contact-us.php":
        $current_page = "Contact Us";
        break;


        // Donation
    case "/donate.php":
        $current_page = "Support Us";
        break;


        // Legal
    case "/legal/cookies.php":
        $current_page = "Cookies Policy";
        break;
    case "/legal/privacy.php":
        $current_page = "Privacy Policy";
        break;
    case "/legal/tos.php":
        $current_page = "Terms of Service";
        break;
}
