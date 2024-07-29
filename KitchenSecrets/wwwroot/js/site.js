// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


<script>
    document.addEventListener('DOMContentLoaded', function () {
    var cards = document.querySelectorAll('.card-custom');
    var maxHeight = 0;

    // Kartların maksimum yüksekliğini bul
    cards.forEach(function(card) {
        card.style.height = 'auto'; // Yüksekliği sıfırla
    var height = card.offsetHeight;
        if (height > maxHeight) {
        maxHeight = height;
        }
    });

    // Kartların yüksekliğini eşitle
    cards.forEach(function(card) {
        card.style.height = maxHeight + 'px';
    });
});

    window.addEventListener('resize', function () {
    var cards = document.querySelectorAll('.card-custom');
    var maxHeight = 0;

    // Kartların maksimum yüksekliğini bul
    cards.forEach(function(card) {
        card.style.height = 'auto'; // Yüksekliği sıfırla
    var height = card.offsetHeight;
        if (height > maxHeight) {
        maxHeight = height;
        }
    });

    // Kartların yüksekliğini eşitle
    cards.forEach(function(card) {
        card.style.height = maxHeight + 'px';
    });
});
</script>
