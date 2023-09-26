using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IChemicalRepository _chemicalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IWarehouseDetailRepository _warehouseDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly INoteRepository _noteRepository;
        private readonly IDepositRepository _depositRepository;

        public UnitOfWork(AppDbContext dbContext,
            IChemicalRepository chemicalRepository,
            IUserRepository userRepository,
            IWarehouseRepository warehouseRepository,
            ICategoryRepository categoryRepository,
            IProviderRepository providerRepository,
            IWarehouseDetailRepository warehouseDetailRepository,
            IOrderRepository orderRepository
            ,INoteRepository noteRepository,
            IDepositRepository depositRepository)
        {
            _dbContext = dbContext;
            _chemicalRepository = chemicalRepository;
            _userRepository = userRepository;
            _warehouseRepository = warehouseRepository;
            _categoryRepository = categoryRepository;
            _providerRepository = providerRepository;
            _warehouseDetailRepository = warehouseDetailRepository;
            _orderRepository = orderRepository;
            _noteRepository = noteRepository;
            _depositRepository = depositRepository;
        }
        public IChemicalRepository ChemicalRepository => _chemicalRepository;

        public IUserRepository UserRepository => _userRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IProviderRepository ProviderRepository => _providerRepository;

        public IWarehouseRepository WarehouseRepository => _warehouseRepository;

        public IWarehouseDetailRepository WarehouseDetailRepository => _warehouseDetailRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public INoteRepository NoteRepository => _noteRepository;

        public IDepositRepository DepositRepository => _depositRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
