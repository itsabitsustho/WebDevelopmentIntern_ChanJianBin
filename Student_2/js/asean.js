const countryData = {
      thailand: {
        title: "🇹🇭 Thai Cuisine",
        subtitle: "Balance of Sweet, Sour, Salty & Spicy",
        dishes: [
          {
            name: "Som Tum",
            icon: "🥗",
            description: "Green papaya salad with lime, chili, and peanuts",
            health: "Rich in fiber and digestive enzymes"
          },
          {
            name: "Tom Yum",
            icon: "🍲",
            description: "Spicy and sour soup with herbs and vegetables",
            health: "Anti-inflammatory herbs boost immunity"
          },
          {
            name: "Larb",
            icon: "🌿",
            description: "Fresh herb salad with lean protein",
            health: "High protein, low carb, nutrient dense"
          }
        ],
        healthBenefits: "Thai cuisine emphasizes fresh herbs like lemongrass, galangal, and Thai basil which have anti-inflammatory and digestive properties. The balance of flavors naturally regulates appetite and supports metabolic health.",
        culturalInfo: "Thai food philosophy centers on achieving harmony in every dish - balancing hot and cold, sweet and salty. This reflects Buddhist principles of moderation and mindful eating."
      },
      malaysia: {
        title: "🇲🇾 Malaysian Cuisine",
        subtitle: "ASEAN Chair 2025 - Unity in Diversity",
        dishes: [
          {
            name: "Ulam with Fish",
            icon: "🌱",
            description: "Raw herb salad with grilled fish and sambal",
            health: "Antioxidant-rich herbs support detoxification"
          },
          {
            name: "Rendang",
            icon: "🍛",
            description: "Slow-cooked beef in coconut and spices",
            health: "Coconut milk provides healthy MCT fats"
          },
          {
            name: "Laksa",
            icon: "🍜",
            description: "Spicy noodle soup with coconut milk",
            health: "Turmeric and galangal offer anti-inflammatory benefits"
          }
        ],
        healthBenefits: "Malaysian cuisine incorporates traditional Malay herbs (ulam) known for their medicinal properties. The use of coconut, turmeric, and pandan leaves provides natural antioxidants and healthy fats.",
        culturalInfo: "As ASEAN Chair 2025, Malaysia exemplifies 'Unity in Diversity' through cuisine that blends Malay, Chinese, and Indian traditions while maintaining distinct cultural identities."
      },
      singapore: {
        title: "🇸🇬 Singaporean Cuisine",
        subtitle: "Hawker Heritage Meets Modern Nutrition",
        dishes: [
          {
            name: "Thunder Tea Rice",
            icon: "🍚",
            description: "Hakka herb tea poured over rice with vegetables",
            health: "Herbal tea provides antioxidants and aids digestion"
          },
          {
            name: "Fish Soup",
            icon: "🐟",
            description: "Clear fish soup with vegetables and tofu",
            health: "High protein, low calories, omega-3 rich"
          },
          {
            name: "Rojak",
            icon: "🥙",
            description: "Fruit and vegetable salad with tamarind dressing",
            health: "Vitamin C rich fruits support immune system"
          }
        ],
        healthBenefits: "Singapore's Healthier Choice Symbol program has transformed hawker culture, promoting dishes lower in sugar, sodium, and saturated fat while maintaining authentic flavors.",
        culturalInfo: "Hawker centers represent Singapore's multicultural harmony - different ethnicities sharing the same space, each contributing their culinary heritage to the nation's food identity."
      },
      indonesia: {
        title: "🇮🇩 Indonesian Cuisine",
        subtitle: "Archipelago of Flavors & Spices",
        dishes: [
          {
            name: "Gado-Gado",
            icon: "🥗",
            description: "Mixed vegetables with peanut sauce",
            health: "Plant-based protein and fiber-rich vegetables"
          },
          {
            name: "Tempeh",
            icon: "🌱",
            description: "Fermented soybean with probiotics",
            health: "Complete protein with beneficial bacteria"
          },
          {
            name: "Sayur Lodeh",
            icon: "🍲",
            description: "Vegetable curry in coconut milk",
            health: "Mixed vegetables provide vitamins and minerals"
          }
        ],
        healthBenefits: "Indonesian cuisine features tempeh and tofu as protein sources, providing plant-based nutrition. The use of turmeric, ginger, and chilies offers natural anti-inflammatory compounds.",
        culturalInfo: "With over 17,000 islands, Indonesian cuisine reflects incredible biodiversity. Each region contributes unique ingredients and cooking methods, unified by the use of aromatic spices."
      },
      vietnam: {
        title: "🇻🇳 Vietnamese Cuisine",
        subtitle: "Fresh, Light & Herb-Abundant",
        dishes: [
          {
            name: "Pho",
            icon: "🍜",
            description: "Aromatic noodle soup with herbs",
            health: "Clear broth aids hydration, herbs boost nutrients"
          },
          {
            name: "Goi Cuon",
            icon: "🥬",
            description: "Fresh spring rolls with herbs and shrimp",
            health: "Raw vegetables preserve maximum vitamins"
          },
          {
            name: "Bun Bo Hue",
            icon: "🌶️",
            description: "Spicy vermicelli soup with lemongrass",
            health: "Lemongrass aids digestion and reduces inflammation"
          }
        ],
        healthBenefits: "Vietnamese cuisine emphasizes fresh herbs consumed raw, preserving maximum nutritional value. Light cooking methods like steaming and quick stir-frying retain vegetable nutrients.",
        culturalInfo: "Vietnamese food philosophy emphasizes 'cân bằng' (balance) - every meal should include vegetables, herbs, and moderate portions to maintain harmony between body and nature."
      },
      philippines: {
        title: "🇵🇭 Filipino Cuisine",
        subtitle: "Island Flavors & Maritime Bounty",
        dishes: [
          {
            name: "Pinakbet",
            icon: "🍆",
            description: "Mixed vegetable stew with bagoong",
            health: "Diverse vegetables provide varied nutrients"
          },
          {
            name: "Sinigang",
            icon: "🍲",
            description: "Sour soup with tamarind and vegetables",
            health: "Tamarind provides vitamin C and antioxidants"
          },
          {
            name: "Kinilaw",
            icon: "🐟",
            description: "Raw fish cured in vinegar with onions",
            health: "Fresh fish provides omega-3 fatty acids"
          }
        ],
        healthBenefits: "Filipino cuisine features abundant seafood providing omega-3 fatty acids. Traditional souring agents like tamarind and kamias are rich in vitamin C and natural probiotics.",
        culturalInfo: "Filipino food reflects the archipelago's maritime culture and history of trade. Each island contributes unique ingredients, creating a cuisine that celebrates both land and sea."
      }
    };

    function openCountryModal(country) {
      const data = countryData[country];
      const modal = document.getElementById('countryModal');
      const modalTitle = document.getElementById('modalTitle');
      const modalSubtitle = document.getElementById('modalSubtitle');
      const modalContent = document.getElementById('modalContent');

      modalTitle.textContent = data.title;
      modalSubtitle.textContent = data.subtitle;

      let dishesHTML = '<div class="dish-gallery">';
      data.dishes.forEach(dish => {
        dishesHTML += `
          <div class="dish-card">
            <div class="dish-icon">${dish.icon}</div>
            <div class="dish-name">${dish.name}</div>
            <div class="dish-description">${dish.description}</div>
            <div style="margin-top: 1rem; padding-top: 1rem; border-top: 1px solid #e5e5e5; font-size: 0.8rem; color: var(--success-green); font-weight: 600;">
              💚 ${dish.health}
            </div>
          </div>
        `;
      });
      dishesHTML += '</div>';

      modalContent.innerHTML = `
        ${dishesHTML}
        <div class="health-benefits">
          <h3 style="color: var(--success-green); margin-bottom: 1rem;">
            <i class="fas fa-heart"></i> Health Benefits
          </h3>
          <p>${data.healthBenefits}</p>
        </div>
        <div class="cultural-info">
          <h3 style="color: var(--accent-gold); margin-bottom: 1rem;">
            <i class="fas fa-globe"></i> Cultural Significance
          </h3>
          <p>${data.culturalInfo}</p>
        </div>
      `;

      modal.style.display = 'block';
      document.body.style.overflow = 'hidden';
    }

    function closeModal() {
      const modal = document.getElementById('countryModal');
      modal.style.display = 'none';
      document.body.style.overflow = 'auto';
    }

    // Close modal when clicking outside
    window.onclick = function (event) {
      const modal = document.getElementById('countryModal');
      if (event.target === modal) {
        closeModal();
      }
    }

    // Animate cards on scroll
    const observerOptions = {
      threshold: 0.1,
      rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry, index) => {
        if (entry.isIntersecting) {
          setTimeout(() => {
            entry.target.classList.add('animate-slide-up');
          }, index * 100);
        }
      });
    }, observerOptions);

    document.querySelectorAll('.country-card').forEach(card => {
      observer.observe(card);
    });

    // Keyboard navigation
    document.addEventListener('keydown', function (e) {
      if (e.key === 'Escape') {
        closeModal();
      }
    });