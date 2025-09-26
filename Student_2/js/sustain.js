// Add hover effects to cards
    document.querySelectorAll('.practice-card, .action-card').forEach(card => {
      card.addEventListener('mouseenter', function() {
        this.style.transform = 'translateY(-10px)';
        this.style.boxShadow = 'var(--shadow-hover)';
      });
      
      card.addEventListener('mouseleave', function() {
        this.style.transform = 'translateY(0)';
        this.style.boxShadow = 'var(--shadow)';
      });
    });

    // Animate statistics on scroll
    const observerOptions = {
      threshold: 0.5,
      rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          const stats = entry.target.querySelectorAll('.impact-stat');
          stats.forEach((stat, index) => {
            setTimeout(() => {
              stat.style.animation = 'fadeInUp 0.8s ease forwards';
            }, index * 200);
          });
        }
      });
    }, observerOptions);

    const impactSection = document.querySelector('.impact-section');
    if (impactSection) {
      observer.observe(impactSection);
    }

    // Smooth scroll for internal links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
      anchor.addEventListener('click', function (e) {
        e.preventDefault();
        document.querySelector(this.getAttribute('href')).scrollIntoView({
          behavior: 'smooth'
        });
      });
    });