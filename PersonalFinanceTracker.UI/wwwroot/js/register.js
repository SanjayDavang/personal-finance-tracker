document.addEventListener('DOMContentLoaded', function () {
    const registerButton = document.getElementById('registerButton');
    const passwordInput = document.getElementById('registerPassword');
    const passwordToggle = document.getElementById('registerPasswordToggle');

    if (registerButton) {
        registerButton.addEventListener('click', function () {
            // Implement registration validation and API call
            console.log("Register button clicked");
        });
    }
    if (passwordToggle && passwordInput) {
        passwordToggle.addEventListener('click', function () {
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                this.innerHTML = '<i class="fas fa-eye-slash"></i>';
            } else {
                passwordInput.type = 'password';
                this.innerHTML = '<i class="fas fa-eye"></i>';
            }
        });
    }
});