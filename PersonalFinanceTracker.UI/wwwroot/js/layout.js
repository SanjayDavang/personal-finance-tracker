document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const switchToRegister = document.getElementById('switchToRegister');
    const switchToLogin = document.getElementById('switchToLogin');

    if (switchToRegister && switchToLogin && loginForm && registerForm) { //Check if elements exist.
        switchToRegister.addEventListener('click', function (e) {
            e.preventDefault();
            loginForm.style.opacity = 0;
            loginForm.style.transform = 'translateY(-20px)';
            setTimeout(() => {
                loginForm.style.display = 'none';
                registerForm.style.display = 'block';
                registerForm.style.opacity = 1;
                registerForm.style.transform = 'translateY(0)';
            }, 300);
        });

        switchToLogin.addEventListener('click', function (e) {
            e.preventDefault();
            registerForm.style.opacity = 0;
            registerForm.style.transform = 'translateY(-20px)';
            setTimeout(() => {
                registerForm.style.display = 'none';
                loginForm.style.display = 'block';
                loginForm.style.opacity = 1;
                loginForm.style.transform = 'translateY(0)';
            }, 300);
        });
    }
});