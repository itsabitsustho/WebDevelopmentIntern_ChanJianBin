// Add hover effects to cards
        document.querySelectorAll('.story-card, .method-card, .preservation-card').forEach(card => {
            card.addEventListener('mouseenter', function () {
                this.style.transform = 'translateY(-10px)';
                this.style.boxShadow = 'var(--shadow-hover)';
            });

            card.addEventListener('mouseleave', function () {
                this.style.transform = 'translateY(0)';
                this.style.boxShadow = 'var(--shadow)';
            });
        });

        // Animate stats on scroll
        const observerOptions = {
            threshold: 0.5,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const stats = entry.target.querySelectorAll('.stat-card');
                    stats.forEach((stat, index) => {
                        setTimeout(() => {
                            stat.style.animation = 'fadeInUp 0.8s ease forwards';
                        }, index * 200);
                    });
                }
            });
        }, observerOptions);

        const heritageStats = document.querySelector('.heritage-stats');
        if (heritageStats) {
            observer.observe(heritageStats);
        }

        // Smooth scroll for internal links
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function (e) {
                e.preventDefault();
                const target = document.querySelector(this.getAttribute('href'));
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth'
                    });
                }
            });
        });