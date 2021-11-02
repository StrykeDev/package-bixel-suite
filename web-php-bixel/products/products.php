<?php
$products_include = true;

function get_products()
{
    return json_decode(file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/products/products.json'), true);
}

function get_product_by_id($id)
{
    $products = get_products();
    foreach ($products as $product) {
        if ($product['id'] == $id) {
            return $product;
        }
    }

    header("Location: /index.php"); 
}
