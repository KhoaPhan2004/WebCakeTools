function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.getElementById('mainContent');
    sidebar.classList.toggle('collapsed');
    mainContent.classList.toggle('collapsed');

    // Collapse submenu when sidebar is collapsed
    if (sidebar.classList.contains('collapsed')) {
        document.getElementById('productsSubmenu')?.classList.remove('show');
        document.getElementById('categoriesSubmenu')?.classList.remove('show');
    }
}

function toggleSubmenu(e) {
    e.preventDefault();
    const targetId = e.currentTarget.getAttribute('data-target');
    const submenu = document.getElementById(targetId);
    submenu?.classList.toggle('show');
}


function createSidebarBubble() {
    const container = document.getElementById('sidebarBackground');
    const bubble = document.createElement('div');
    bubble.classList.add('bubble');

    const size = Math.random() * 30 + 10;
    bubble.style.width = size + 'px';
    bubble.style.height = size + 'px';
    bubble.style.left = Math.random() * 100 + '%';
    bubble.style.animationDuration = (Math.random() * 4 + 4) + 's';

    container.appendChild(bubble);
    setTimeout(() => container.removeChild(bubble), 10000);
}

setInterval(createSidebarBubble, 300);















//#262932, #1d1e26, #262932, #374558
//#262932, #1d1e26


//function toggleTheme() {
//    const mainContent = document.getElementById("mainContent");
//    const icon = document.getElementById("themeIcon");

//    const isDark = mainContent.classList.toggle("bg-black");

//    // Lưu vào localStorage
//    localStorage.setItem("theme", isDark ? "dark" : "light");

//    // Đổi icon
//    icon.classList.remove(isDark ? "bi-moon-fill" : "bi-sun-fill");
//    icon.classList.add(isDark ? "bi-sun-fill" : "bi-moon-fill");
//}

//// Khi load lại trang thì giữ theme cũ
//window.addEventListener("DOMContentLoaded", () => {
//    const mainContent = document.getElementById("mainContent");
//    const icon = document.getElementById("themeIcon");

//    const savedTheme = localStorage.getItem("theme");
//    if (savedTheme === "dark") {
//        mainContent.classList.add("bg-black");
//        icon.classList.remove("bi-moon-fill");
//        icon.classList.add("bi-sun-fill");
//    }
//});

function toggleTheme() {
    const mainContent = document.getElementById("mainContent");
    const icon = document.getElementById("themeIcon");

    const isDark = mainContent.classList.toggle("dark-mode");

    // Lưu vào localStorage
    localStorage.setItem("theme", isDark ? "dark" : "light");

    // Đổi icon
    icon.classList.remove(isDark ? "bi-moon-fill" : "bi-sun-fill");
    icon.classList.add(isDark ? "bi-sun-fill" : "bi-moon-fill");

    // Đổi màu bảng
    document.querySelectorAll(".table-responsive").forEach(el => {
        el.style.backgroundColor = isDark ? "#262932" : "";
    });
}

// Khi load lại trang thì giữ theme cũ
window.addEventListener("DOMContentLoaded", () => {
    const mainContent = document.getElementById("mainContent");
    const icon = document.getElementById("themeIcon");
    const savedTheme = localStorage.getItem("theme");

    if (savedTheme === "dark") {
        mainContent.classList.add("dark-mode");
        icon.classList.remove("bi-moon-fill");
        icon.classList.add("bi-sun-fill");

        document.querySelectorAll(".table-responsive").forEach(el => {
            el.style.backgroundColor = "#262932";
        });
    }
});









// Gọi alert tự động khi TempData tồn tại
window.addEventListener('DOMContentLoaded', () => {
    showAlert();
});




let alertTimeout;

function showAlert() {
    const alert = document.getElementById("alertBox");

    // Reset timer animation
    const timerBar = alert.querySelector(".alert-timer");
    timerBar.style.animation = "none";
    void timerBar.offsetWidth; // trigger reflow
    timerBar.style.animation = "countdown 5s linear forwards";

    alert.classList.add("show");

    clearTimeout(alertTimeout);
    alertTimeout = setTimeout(() => {
        hideAlert();
    }, 5000);
}

function hideAlert() {
    const alert = document.getElementById("alertBox");
    alert.classList.remove("show");
}










document.querySelector('form').addEventListener('submit', function (e) {
    const submitButton = this.querySelector('input[type="submit"]');
    submitButton.disabled = true;
    submitButton.value = "Đang xử lý..."; // tuỳ chọn
});