

    document.addEventListener('mousemove', (e) => {
        const sprinkle = document.createElement('div');
    sprinkle.classList.add('sprinkle');

    // Màu pastel ngẫu nhiên
    const colors = ['#f7c6c7', '#fcd5ce', '#f9e0bb', '#cdb4db', '#d2f6c5'];
    sprinkle.style.backgroundColor = colors[Math.floor(Math.random() * colors.length)];

    // Vị trí xuất hiện
    sprinkle.style.left = e.clientX + 'px';
    sprinkle.style.top = e.clientY + 'px';

    document.body.appendChild(sprinkle);

        // Xoá sau khi hiệu ứng kết thúc
        setTimeout(() => {
        sprinkle.remove();
        }, 600);
    });



















document.querySelectorAll('.product-card').forEach(card => {
    const MAX_ROTATE = 15; // độ nghiêng tối đa (độ)

    card.addEventListener('mousemove', (e) => {
        const rect = card.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const centerX = rect.width / 2;
        const rotateY = ((x - centerX) / centerX) * MAX_ROTATE;

        card.style.transform = `scale(1.03) rotateY(${rotateY}deg)`;
    });

    card.addEventListener('mouseleave', () => {
        card.style.transform = 'scale(1)';
    });
});






const banners = document.querySelectorAll('.carousel-item');

banners.forEach(banner => {
    const content = banner.querySelector('.banner-content .inner');

    banner.addEventListener('mousemove', e => {
        const rect = banner.getBoundingClientRect();
        const x = (e.clientX - rect.left) / rect.width - 0.5;
        const y = (e.clientY - rect.top) / rect.height - 0.5;

        content.style.transform = `translate(${-x * 60}px, ${-y * 30}px)`;
    });

    banner.addEventListener('mouseleave', () => {
        content.style.transform = 'translate(0,0)';
    });
});







function setActiveTab(clicked) {
    // Tìm vùng chứa .category-tabs gần nhất (container DS hiện tại)
    const container = clicked.closest('.container');

    // Xóa class active của các tab-button trong vùng hiện tại
    container.querySelectorAll('.tab-button').forEach(btn => {
        btn.classList.remove('active');
    });

    // Gán class active cho nút đang được click
    clicked.classList.add('active');

    // Lấy categoryId từ nút
    const categoryId = clicked.getAttribute("data-id");

    // Tìm phần tử chứa sản phẩm tương ứng với vùng hiện tại
    const productList = container.nextElementSibling.querySelector('#product-list-container');

    // Gọi ViewComponent bằng fetch
    fetch(`/Product/GetProductsByCategory?categoryId=${categoryId}`)
        .then(res => res.text())
        .then(html => {
            productList.innerHTML = html;
        });
}








//const customSlider = document.querySelector('.custom-slider');
//document.querySelector('.custom-next').addEventListener('click', () => {
//    customSlider.scrollBy({ left: 320, behavior: 'smooth' });
//});

//document.querySelector('.custom-prev').addEventListener('click', () => {
//    customSlider.scrollBy({ left: -320, behavior: 'smooth' });
//});

//// Hover nghiêng
//document.querySelectorAll('.custom-card').forEach(card => {
//    const inner = card.querySelector('.custom-inner');
//    const MAX_ROTATE = 15;

//    card.addEventListener('mousemove', (e) => {
//        const rect = card.getBoundingClientRect();
//        const x = e.clientX - rect.left;
//        const y = e.clientY - rect.top;
//        const centerX = rect.width / 2;
//        const centerY = rect.height / 2;
//        const rotateX = -((y - centerY) / centerY) * MAX_ROTATE;
//        const rotateY = ((x - centerX) / centerX) * MAX_ROTATE;

//        inner.style.transform = `rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.03)`;
//    });

//    card.addEventListener('mouseleave', () => {
//        inner.style.transform = 'rotateX(0deg) rotateY(0deg) scale(1)';
//    });
//});



















const slider = document.querySelector('.scroll-product-wrapper');
const track = document.querySelector('.scroll-product-track');

let isDown = false;
let startX;
let scrollLeft;

const scrollSpeed = 1;

// Clone nội dung track để tạo vòng lặp (nếu chưa clone)
// Nếu bạn clone rồi trong HTML thì bỏ đoạn này
track.innerHTML += track.innerHTML;

function autoScroll() {
    if (!isDown) {
        slider.scrollLeft += scrollSpeed;

        // Khi scroll tới nửa track (do nhân đôi), reset scrollLeft về 0 mượt mà
        if (slider.scrollLeft >= track.scrollWidth / 2) {
            slider.scrollLeft = 0;
        }
    }
    requestAnimationFrame(autoScroll);
}

slider.addEventListener('mousedown', (e) => {
    isDown = true;
    slider.classList.add('active');
    startX = e.pageX - slider.offsetLeft;
    scrollLeft = slider.scrollLeft;
});

slider.addEventListener('mouseleave', () => {
    isDown = false;
    slider.classList.remove('active');
});

slider.addEventListener('mouseup', () => {
    isDown = false;
    slider.classList.remove('active');
});

slider.addEventListener('mousemove', (e) => {
    if (!isDown) return;
    e.preventDefault();
    const x = e.pageX - slider.offsetLeft;
    const walk = (x - startX) * 2;
    slider.scrollLeft = scrollLeft - walk;
});

autoScroll();




























function cakeScroll(direction) {
    const container = document.querySelector('.cake-slider-track');
    const item = container.querySelector('.cake-card-item');

    if (item) {
        const itemWidth = item.offsetWidth + parseInt(getComputedStyle(container).gap || 16); // gồm cả khoảng cách giữa các item
        container.scrollBy({
            left: direction * itemWidth,
            behavior: 'smooth'
        });
    }
}

