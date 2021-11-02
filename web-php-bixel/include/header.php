<header class="shadow-box">
    <nav class="navbar navbar-expand-sm container">

        <!-- brand-->
        <a class="navbar-brand" href="/index.php">
            <img src="/assats/brand/logo_bixel_w_x256.png" alt="Bixel" height="50px" class="can-invert">
        </a>

        <!-- collapse button -->
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" viewBox="0 0 192 192" width="42px" height="42px">
                <g fill="#7f7f7f">
                    <path d="M24,88h144v16h-144z" />
                    <path d="M24,40h144v16h-144z" />
                    <path d="M24,136h144v16h-144z" />
                </g>
            </svg>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">

                <!-- producats -->
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Products
                    </a>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                        <?php
                        if (!isset($products_include))
                            include($_SERVER['DOCUMENT_ROOT'] . '/products/products.php');

                        $items = get_products();

                        foreach ($items as $item) {
                            echo '<a class="dropdown-item" href="/product.php?id=' . $item['id'] . '">' . $item['name'] . '</a>';
                        }
                        ?>
                    </div>
                </li>

                <!-- support center -->
                <li class="nav-item">
                    <a class="nav-link" href="/support-center.php">Support Center</a>
                </li>
            </ul>

            <hr>
            <form class="form-inline">
                <ul class="d-flex p-0 mb-0" style="list-style:none; ">

                    <!-- theme switch -->
                    <li>
                        <button type="button" onclick="switchTheme()" id="theme-icon" class="btn mr-2"></button>
                        <script>
                            function switchTheme() {
                                var html = document.getElementsByTagName('html')[0];
                                document.getElementsByTagName('body')[0].classList.add('transition');

                                var d = new Date();
                                d.setTime(d.getTime() + (365 * 86400000));

                                console.log(d.toUTCString());

                                if (html.getAttribute("data-theme") == "dark") {
                                    html.setAttribute("data-theme", "light");
                                    document.cookie = "theme=light;  expires=" + d.toUTCString() + "; path=/";
                                } else {
                                    html.setAttribute("data-theme", "dark");
                                    document.cookie = "theme=dark;  expires=" + d.toUTCString() + "; path=/";
                                }

                                setTimeout(function() {
                                    document.getElementsByTagName('body')[0].classList.remove('transition');
                                }, 300);

                                return false;
                            }
                        </script>
                    </li>

                    <!-- donate button -->
                    <li>
                        <a href="/donate.php" class="btn">Support Us</a>
                    </li>
                </ul>
            </form>
        </div>
    </nav>
</header>